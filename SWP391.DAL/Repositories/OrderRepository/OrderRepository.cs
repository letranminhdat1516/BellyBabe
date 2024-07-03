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

            var processingStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Chờ xác nhận");
            if (processingStatus == null)
            {
                throw new Exception("Không tìm thấy trạng thái xử lý.");
            }

            var orderDetails = await _context.OrderDetails
                .Where(od => od.UserId == userId && od.IsChecked == true && od.OrderId == null)
                .ToListAsync();

            if (!orderDetails.Any())
            {
                throw new Exception("Không có sản phẩm được chọn trong giỏ hàng.");
            }

            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery == null)
            {
                throw new Exception("Không tìm thấy phương thức giao hàng.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderStatuses = new List<OrderStatus> { processingStatus },
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

            order.OrderStatuses.Clear();
            order.OrderStatuses.Add(status);

            // Save the updated order status
            await _context.SaveChangesAsync();

            // Update cumulative score if the order status is "Đã giao hàng"
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
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new ArgumentException("ID đơn hàng không hợp lệ.");
                }

                var cancelStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã hủy");
                if (cancelStatus == null)
                {
                    throw new Exception("Không tìm thấy trạng thái hủy.");
                }

                var currentStatus = order.OrderStatuses.FirstOrDefault();
                if (currentStatus == null)
                {
                    throw new Exception("Không tìm thấy trạng thái đơn hàng hiện tại.");
                }

                if (order.OrderStatuses.Any(os => os.StatusId == cancelStatus.StatusId))
                {
                    throw new InvalidOperationException("Đơn hàng đã bị hủy.");
                }

                var nonCancelableStatuses = new List<string> { "Đã giao hàng", "Đang giao hàng" };
                if (nonCancelableStatuses.Contains(currentStatus.StatusName))
                {
                    throw new InvalidOperationException("Không thể hủy đơn hàng đã được giao hoặc đang giao.");
                }

                order.OrderStatuses.Clear();
                order.OrderStatuses.Add(cancelStatus);

                var orderDetails = await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.ProductId.HasValue && orderDetail.Quantity.HasValue)
                    {
                        await _productRepository.UpdateProductQuantity(orderDetail.ProductId.Value, -orderDetail.Quantity.Value);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<OrderModel>> GetAllOrders()
        {
            var listOfOrders = await _context.Orders
                .Select(o => new OrderModel
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
                    OrderStatuses = o.OrderStatuses.Select(os => new OrderStatus
                    {
                        StatusId = os.StatusId,
                        StatusName = os.StatusName,
                        OrderId = os.OrderId
                    }).ToList()
                })
                .ToListAsync();

            return listOfOrders;
        }
    }
}
