using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Order;
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
        private readonly CartRepository _cartRepository;
        private readonly CumulativeScoreRepository.CumulativeScoreRepository _cumulativeScoreRepository;

        public OrderRepository(Swp391Context context,
                               ProductRepository.ProductRepository productRepository,
                               CartRepository cartRepository,
                               CumulativeScoreRepository.CumulativeScoreRepository cumulativeScoreRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cumulativeScoreRepository = cumulativeScoreRepository;
        }

        public async Task<Order> PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string note)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            var orderDetails = await _cartRepository.GetCartDetailsAsync(userId);

            if (!orderDetails.Any())
            {
                throw new Exception("Không có sản phẩm trong giỏ hàng.");
            }

            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery == null)
            {
                throw new Exception("Không tìm thấy phương thức giao hàng.");
            }

            var order = new Order
            {
                UserId = userId,
                RecipientName = recipientName,
                RecipientPhone = recipientPhone,
                RecipientAddress = recipientAddress,
                Note = note,
                OrderDate = DateTime.Now,
                TotalPrice = orderDetails.Sum(od => od.Price)
            };

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.UserId = null;

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

                // Add the "Chờ xác nhận" status directly to the order
                var processingStatus = new OrderStatus
                {
                    StatusName = "Chờ xác nhận",
                    OrderId = order.OrderId,
                    StatusUpdateDate = DateTime.Now
                };

                _context.OrderStatuses.Add(processingStatus);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Có lỗi xảy ra khi đặt hàng", ex);
            }

            return order;
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderStatuses)
                .Include(o => o.Deliveries)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string statusName)
        {
            var order = await _context.Orders
                .Include(o => o.OrderStatuses)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            // Check if the order has the "Chờ xác nhận" status
            var processingStatus = order.OrderStatuses.FirstOrDefault(os => os.StatusName == "Chờ xác nhận");
            if (processingStatus == null)
            {
                throw new InvalidOperationException("Chỉ có thể cập nhật trạng thái đơn hàng khi trạng thái hiện tại là 'Chờ xác nhận'.");
            }

            // Add or update the status
            var existingStatus = order.OrderStatuses.FirstOrDefault(os => os.StatusName == statusName);
            if (existingStatus != null)
            {
                existingStatus.StatusUpdateDate = DateTime.Now;
            }
            else
            {
                order.OrderStatuses.Add(new OrderStatus
                {
                    OrderId = orderId,
                    StatusName = statusName,
                    StatusUpdateDate = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            // Update cumulative score if the status is "Đã giao hàng"
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
                .Where(o => o.UserId == userId && o.OrderStatuses.Any(os => os.StatusId == status.StatusId))
                .Include(o => o.OrderStatuses)
                .Include(o => o.Deliveries)
                .ToListAsync();
        }

        public async Task CancelOrderAsync(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderStatuses)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    throw new ArgumentException("ID đơn hàng không hợp lệ.");
                }

                // Check if the order has the "Chờ xác nhận" status
                var processingStatus = order.OrderStatuses.FirstOrDefault(os => os.StatusName == "Chờ xác nhận");
                if (processingStatus == null)
                {
                    throw new InvalidOperationException("Chỉ có thể hủy đơn hàng khi trạng thái hiện tại là 'Chờ xác nhận'.");
                }

                // Set the order status to "Đã hủy"
                var cancelStatus = new OrderStatus
                {
                    StatusName = "Đã hủy",
                    OrderId = orderId,
                    StatusUpdateDate = DateTime.Now
                };

                order.OrderStatuses.Add(cancelStatus);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Có lỗi xảy ra khi hủy đơn hàng", ex);
            }
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.OrderStatuses)
                .ToListAsync();

            var listOfOrders = orders.Select(o => new OrderModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                Note = o.Note,
                VoucherId = o.VoucherId,
                TotalPrice = o.TotalPrice,
                OrderDate = o.OrderDate,
                RecipientName = o.RecipientName,
                RecipientPhone = o.RecipientPhone,
                RecipientAddress = o.RecipientAddress,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetail
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList(),
                OrderStatuses = o.OrderStatuses.Any() 
                  ? o.OrderStatuses.Select(os => new OrderStatus
                  {
                      StatusId = os.StatusId,
                      StatusName = os.StatusName,
                      OrderId = os.OrderId,
                      StatusUpdateDate = os.StatusUpdateDate
                  }).ToList()
                  : new List<OrderStatus> { new OrderStatus { StatusName = "Chờ xác nhận" } } 
            }).ToList();

            return listOfOrders;
        }
    }
}
