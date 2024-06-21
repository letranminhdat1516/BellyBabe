using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.OrderRepository
{
    public class OrderRepository
    {
        private readonly Swp391Context _context;
        private readonly ProductRepository.ProductRepository _productRepository;
        private readonly CumulativeScoreRepository.CumulativeScoreRepository _cumulativeScoreRepository;

        public OrderRepository(Swp391Context context, ProductRepository.ProductRepository productRepository, CumulativeScoreRepository.CumulativeScoreRepository cumulativeScoreRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _cumulativeScoreRepository = cumulativeScoreRepository;
        }

        public async Task PlaceOrderAsync(int userId, string address, int deliveryId, string paymentMethod, string phoneNumber, string note)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            var processingStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Chờ xác nhận");
            if (processingStatus == null)
            {
                throw new Exception("Processing status not found.");
            }

            var orderDetails = await _context.OrderDetails.Where(od => od.UserId == userId && od.OrderId == null).ToListAsync();
            if (!orderDetails.Any())
            {
                throw new Exception("No items in the cart.");
            }

            var delivery = await _context.Deliveries.FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
            if (delivery == null)
            {
                throw new Exception("Delivery method not found.");
            }

            var order = new Order
            {
                UserId = userId,
                StatusId = processingStatus.StatusId,
                Address = address,
                DeliveryId = deliveryId,
                DeliveryMethod = delivery.DeliveryMethod,
                DeliveryFee = delivery.DeliveryFee,
                PaymentMethod = paymentMethod,
                PhoneNumber = phoneNumber,
                Note = note,
                OrderDate = DateTime.Now,
                TotalPrice = orderDetails.Sum(od => od.Price * od.Quantity)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var orderDetail in orderDetails)
            {
                orderDetail.OrderId = order.OrderId;
                orderDetail.UserId = null;

                // Update product quantity
                if (orderDetail.ProductId.HasValue && orderDetail.Quantity.HasValue)
                {
                    await _productRepository.UpdateProductQuantity(orderDetail.ProductId.Value, orderDetail.Quantity.Value);
                }
            }

            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                OrderId = order.OrderId,
                PayTime = DateTime.Now,
                Amount = order.TotalPrice ?? 0,
                ExternalTransactionCode = "ExternalCode"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string statusName)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException("Invalid order ID.");
            }

            var status = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
            if (status == null)
            {
                throw new ArgumentException("Invalid status name.");
            }

            order.StatusId = status.StatusId;
            await _context.SaveChangesAsync();

            if (statusName == "Đã giao hàng")
            {
                await _cumulativeScoreRepository.UpdateCumulativeScoreAsync(order.UserId ?? 0);
            }
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int userId, string statusName)
        {
            var status = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
            if (status == null)
            {
                throw new ArgumentException("Invalid status name.");
            }

            return await _context.Orders
                .Where(o => o.UserId == userId && o.StatusId == status.StatusId)
                .ToListAsync();
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException("Invalid order ID.");
            }

            var status = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã hủy");
            if (status == null)
            {
                throw new Exception("Cancel status not found.");
            }

            if (order.StatusId != status.StatusId)
            {
                order.StatusId = status.StatusId;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Order is already cancelled.");
            }
        }
    }
}
