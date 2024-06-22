using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string paymentMethod, string note)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            var processingStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Chờ xác nhận");
            if (processingStatus == null)
            {
                throw new Exception("Không tìm thấy trạng thái xử lý.");
            }

            var orderDetails = await _context.OrderDetails.Where(od => od.UserId == userId && od.OrderId == null).ToListAsync();
            if (!orderDetails.Any())
            {
                throw new Exception("Không có sản phẩm trong giỏ hàng.");
            }

            var delivery = await _context.Deliveries.FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
            if (delivery == null)
            {
                throw new Exception("Không tìm thấy phương thức giao hàng.");
            }

            var order = new Order
            {
                UserId = userId,
                StatusId = processingStatus.StatusId,
                RecipientName = recipientName,
                RecipientPhone = recipientPhone,
                RecipientAddress = recipientAddress,
                DeliveryId = deliveryId,
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

                // Cập nhật số lượng sản phẩm
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
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            var status = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
            if (status == null)
            {
                throw new ArgumentException("Tên trạng thái không hợp lệ.");
            }

            order.StatusId = status.StatusId;
            await _context.SaveChangesAsync();

            if (statusName == "Đã giao hàng")
            {
                await _cumulativeScoreRepository.UpdateCumulativeScoreAsync(order.UserId);
            }
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int userId, string statusName)
        {
            var status = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
            if (status == null)
            {
                throw new ArgumentException("Tên trạng thái không hợp lệ.");
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
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            // Get the 'Đã hủy' (Cancelled) status
            var cancelStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã hủy");
            if (cancelStatus == null)
            {
                throw new Exception("Không tìm thấy trạng thái hủy.");
            }

            // Get the current status of the order
            var currentStatus = await _context.OrderStatuses.FindAsync(order.StatusId);
            if (currentStatus == null)
            {
                throw new Exception("Không tìm thấy trạng thái đơn hàng hiện tại.");
            }

            // Check if the order is already canceled
            if (order.StatusId == cancelStatus.StatusId)
            {
                throw new InvalidOperationException("Đơn hàng đã bị hủy.");
            }

            // Add logic to prevent cancellation of orders that are already delivered or in process
            var nonCancelableStatuses = new List<string> { "Đã giao hàng", "Đang giao hàng" };
            if (nonCancelableStatuses.Contains(currentStatus.StatusName))
            {
                throw new InvalidOperationException("Không thể hủy đơn hàng đã được giao hoặc đang giao.");
            }

            // Update the status of the order to 'Đã hủy' (Cancelled)
            order.StatusId = cancelStatus.StatusId;
            await _context.SaveChangesAsync();

            // Logic to revert product quantities
            var orderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
            foreach (var orderDetail in orderDetails)
            {
                if (orderDetail.ProductId.HasValue && orderDetail.Quantity.HasValue)
                {
                    await _productRepository.UpdateProductQuantity(orderDetail.ProductId.Value, -orderDetail.Quantity.Value);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
