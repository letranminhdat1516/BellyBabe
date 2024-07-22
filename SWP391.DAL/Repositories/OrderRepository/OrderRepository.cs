using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Order;
using SWP391.DAL.Model.Product;
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
        private readonly CumulativeScoreTransactionRepository.CumulativeScoreTransactionRepository _cumulativeScoreTransactionRepository;

        public OrderRepository(Swp391Context context,
                               ProductRepository.ProductRepository productRepository,
                               CartRepository cartRepository,
                               CumulativeScoreRepository.CumulativeScoreRepository cumulativeScoreRepository,
                               CumulativeScoreTransactionRepository.CumulativeScoreTransactionRepository cumulativeScoreTransactionRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cumulativeScoreRepository = cumulativeScoreRepository;
            _cumulativeScoreTransactionRepository = cumulativeScoreTransactionRepository;
        }

        public async Task<Order> PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string? note, bool? usePoints = null)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            var orderDetails = await _cartRepository.GetCartDetailsAsync(userId);
            var checkedOrderDetails = orderDetails.Where(od => od.IsChecked == true).ToList();

            if (!checkedOrderDetails.Any())
            {
                throw new Exception("Không có sản phẩm được chọn trong giỏ hàng.");
            }

            var delivery = await _context.DeliveryMethods.FindAsync(deliveryId);
            if (delivery == null)
            {
                throw new Exception("Không tìm thấy phương thức giao hàng.");
            }

            int subtotal = orderDetails.Sum(od => od.Price ?? 0);
            int deliveryFee = delivery.DeliveryFee ?? 0;
            int totalPrice = subtotal + deliveryFee;

            int? pointsToUse = null;
            if (usePoints == true)
            {
                int availablePoints = await _cumulativeScoreRepository.GetUserCumulativeScoreAsync(userId);
                pointsToUse = Math.Min(availablePoints, totalPrice);
            }


            int finalPrice = pointsToUse.HasValue ? Math.Max(totalPrice - pointsToUse.Value, 0) : totalPrice;

            var order = new Order
            {
                UserId = userId,
                RecipientName = recipientName,
                RecipientPhone = recipientPhone,
                RecipientAddress = recipientAddress,
                Note = note,
                OrderDate = DateTime.Now,
                TotalPrice = finalPrice,
                PointsUsed = pointsToUse
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderDelivery = new DeliveryMethod
                {
                    OrderId = order.OrderId,
                    DeliveryName = delivery.DeliveryName,
                    DeliveryFee = delivery.DeliveryFee
                };
                _context.DeliveryMethods.Add(orderDelivery);

                foreach (var orderDetail in checkedOrderDetails)
                {
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.UserId = null;

                    if (orderDetail.ProductId.HasValue && orderDetail.Quantity.HasValue)
                    {
                        await _productRepository.UpdateProductQuantity(orderDetail.ProductId.Value, orderDetail.Quantity.Value);
                    }
                }

                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    PayTime = DateTime.Now,
                    Amount = finalPrice,
                    ExternalTransactionCode = "ExternalCode"
                };

                _context.Payments.Add(payment);

                // Update user's cumulative score if points were used
                if (pointsToUse.HasValue && pointsToUse.Value > 0)
                {
                    await _cumulativeScoreRepository.UsePointsAsync(userId, pointsToUse.Value);
                    await _cumulativeScoreTransactionRepository.AddTransactionAsync(userId, order.OrderId, -pointsToUse.Value, "PointsUsed");
                }

                // Add the "Chờ xác nhận" status directly to the order
                var processingStatus = new OrderStatus
                {
                    StatusName = "Chờ xác nhận",
                    OrderId = order.OrderId,
                    StatusUpdateDate = DateTime.Now
                };

                _context.OrderStatuses.Add(processingStatus);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Có lỗi xảy ra khi đặt hàng", ex);
            }

            return order;
        }
    
    public async Task UpdateOrderStatusAsync(int orderId, string statusName)
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
                    int scoreToAdd = CalculateScoreToAdd(order.TotalPrice.GetValueOrDefault());
                    await _cumulativeScoreRepository.AddPointsAsync(order.UserId, scoreToAdd);
                    await _cumulativeScoreTransactionRepository.AddTransactionAsync(order.UserId, order.OrderId, scoreToAdd, "OrderCompleted");

                    // Log for debugging
                    Console.WriteLine("Cumulative score updated.");
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Có lỗi xảy ra khi cập nhật trạng thái đơn hàng", ex);
            }
        }
        private int CalculateScoreToAdd(int totalPrice)
        {
            // You can adjust this calculation based on your business rules
            return totalPrice / 1000;
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
                .Include(o => o.DeliveryMethods)
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
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailModel
                {
                    ProductId = od.ProductId,
                    Price = od.Price,
                    Quantity = od.Quantity
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

        public async Task<OrderStatus> GetLatestOrderStatusAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderStatuses)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            var latestStatus = order.OrderStatuses
                .OrderByDescending(os => os.StatusUpdateDate)
                .FirstOrDefault();

            if (latestStatus == null)
            {
                throw new InvalidOperationException("Không tìm thấy trạng thái đơn hàng.");
            }

            return latestStatus;
        }

        public async Task<OrderModel> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Include(o => o.OrderStatuses)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            var orderModel = new OrderModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Note = order.Note,
                VoucherId = order.VoucherId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                RecipientAddress = order.RecipientAddress,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailModel
                {
                    ProductId = od.ProductId,
                    Price = od.Price,
                    Quantity = od.Quantity,
                    Product = new ProductModel
                    {
                        ProductId = od.Product?.ProductId ?? 0,
                        ProductName = od.Product?.ProductName,
                        Description = od.Product?.Description,
                        Quantity = od.Product?.Quantity ?? 0,
                        IsSoldOut = od.Product?.IsSoldOut ?? 0,
                        BackInStockDate = od.Product?.BackInStockDate,
                        CategoryId = od.Product?.CategoryId,
                        BrandId = od.Product?.BrandId,
                        FeedbackTotal = od.Product?.FeedbackTotal,
                        OldPrice = od.Product?.OldPrice,
                        Discount = od.Product?.Discount,
                        NewPrice = od.Product?.NewPrice,
                        ImageLinks = od.Product?.ImageLinks,
                        Brand = od.Product?.Brand,
                        Category = od.Product?.Category
                    }
                }).ToList(),
                OrderStatuses = order.OrderStatuses.Select(os => new OrderStatus
                {
                    StatusId = os.StatusId,
                    StatusName = os.StatusName,
                    OrderId = os.OrderId,
                    StatusUpdateDate = os.StatusUpdateDate
                }).ToList()
            };

            return orderModel;
        }

        public async Task<List<OrderModel>> GetOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) 
                .Include(o => o.OrderStatuses)
                .Include(o => o.DeliveryMethods)
                .ToListAsync();

            var orderModels = orders.Select(order => new OrderModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Note = order.Note,
                VoucherId = order.VoucherId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                RecipientAddress = order.RecipientAddress,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailModel
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    Product = new ProductModel
                    {
                        ProductId = od.Product?.ProductId ?? 0,
                        ProductName = od.Product?.ProductName,
                        Description = od.Product?.Description,
                        Quantity = od.Product?.Quantity ?? 0,
                        IsSoldOut = od.Product?.IsSoldOut ?? 0,
                        BackInStockDate = od.Product?.BackInStockDate,
                        CategoryId = od.Product?.CategoryId,
                        BrandId = od.Product?.BrandId,
                        FeedbackTotal = od.Product?.FeedbackTotal,
                        OldPrice = od.Product?.OldPrice,
                        Discount = od.Product?.Discount,
                        NewPrice = od.Product?.NewPrice,
                        ImageLinks = od.Product?.ImageLinks,
                        Brand = od.Product?.Brand,
                        Category = od.Product?.Category
                    }
                }).ToList(),
                OrderStatuses = order.OrderStatuses.Select(os => new OrderStatus
                {
                    StatusId = os.StatusId,
                    StatusName = os.StatusName,
                    OrderId = os.OrderId,
                    StatusUpdateDate = os.StatusUpdateDate
                }).ToList()
            }).ToList();

            return orderModels;
        }
    }
}
