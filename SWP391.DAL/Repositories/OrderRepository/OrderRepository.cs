using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Order;
using SWP391.DAL.Model.Product;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        private bool IsValidVietnameseAddress(string address)
        {
            string vietnameseAddressPattern = @"^[a-zA-Z0-9\s,.-áàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễíìịỉĩóòọỏõôốồộổỗơớờợởỡúùụủũưứừựửữýỳỵỷỹđÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴÉÈẸẺẼÊẾỀỆỂỄÍÌỊỈĨÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠÚÙỤỦŨƯỨỪỰỬỮÝỲỴỶỸĐ]+$";
            return Regex.IsMatch(address, vietnameseAddressPattern);
        }

        public async Task<Order> PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, string? note, bool? usePoints = null)
        {
            if (string.IsNullOrEmpty(recipientPhone) || !Regex.IsMatch(recipientPhone, @"^[0-9]{1,11}$"))
            {
                throw new ArgumentException("Số điện thoại không hợp lệ.");
            }

            if (string.IsNullOrEmpty(recipientAddress) || !IsValidVietnameseAddress(recipientAddress))
            {
                throw new ArgumentException("Địa chỉ không hợp lệ. Vui lòng nhập địa chỉ hợp lệ bằng tiếng Việt.");
            }

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

            int subtotal = checkedOrderDetails.Sum(od => od.Price ?? 0);
            int deliveryFee = subtotal > 1500000 ? 0 : 30000;
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
                PointsUsed = pointsToUse,
                StatusId = 1 // Chờ xác nhận
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

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

            if (pointsToUse.HasValue && pointsToUse.Value > 0)
            {
                await _cumulativeScoreRepository.UsePointsAsync(userId, pointsToUse.Value);
                await _cumulativeScoreTransactionRepository.AddTransactionAsync(userId, order.OrderId, -pointsToUse.Value, "PointsUsed");
            }

            var orderStatusHistory = new OrderStatusHistory
            {
                OrderId = order.OrderId,
                UserId = userId,
                StatusId = 1, // Chờ xác nhận
                UpdatedDate = DateTime.Now
            };

            _context.OrderStatusHistories.Add(orderStatusHistory);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, int statusId, string? note = null)
        {
            var order = await _context.Orders
                .Include(o => o.OrderStatusHistories)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            var orderStatusHistory = new OrderStatusHistory
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                StatusId = statusId,
                UpdatedDate = DateTime.Now,
                Note = note
            };

            order.StatusId = statusId;

            _context.OrderStatusHistories.Add(orderStatusHistory);
            await _context.SaveChangesAsync();

            if (statusId == 6) // Đã nhận hàng
            {
                int scoreToAdd = CalculateScoreToAdd((int)order.TotalPrice.GetValueOrDefault());
                await _cumulativeScoreRepository.AddPointsAsync(order.UserId, scoreToAdd);
                await _cumulativeScoreTransactionRepository.AddTransactionAsync(order.UserId, order.OrderId, scoreToAdd, "OrderCompleted");
            }
        }

        private int CalculateScoreToAdd(int totalPrice)
        {
            return totalPrice / 1000;
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int userId, int statusId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId && o.StatusId == statusId)
                .Include(o => o.OrderStatusHistories).ThenInclude(osh => osh.Status)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            return orders.Select(order => new Order
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
                OrderDetails = order.OrderDetails,
                OrderStatusHistories = order.OrderStatusHistories.Select(osh => new OrderStatusHistory
                {
                    StatusId = osh.StatusId,
                    OrderId = osh.OrderId,
                    UserId = osh.UserId,
                    UpdatedDate = osh.UpdatedDate,
                    Note = osh.Note,
                    Status = osh.Status != null ? new OrderStatus
                    {
                        StatusId = osh.Status.StatusId,
                        StatusName = osh.Status.StatusName
                    } : null
                }).ToList()
            }).ToList();
        }

        public async Task CancelOrderAsync(int orderId, string reason)
        {
            var order = await _context.Orders
                .Include(o => o.OrderStatusHistories)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            if (order.StatusId != 1)
            {
                throw new InvalidOperationException("Chỉ có thể hủy đơn hàng khi trạng thái hiện tại là 'Chờ xác nhận'.");
            }

            // Update the product quantities
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.ProductId.HasValue && orderDetail.Quantity.HasValue)
                {
                    await _productRepository.ReturnProductQuantity(orderDetail.ProductId.Value, orderDetail.Quantity.Value);
                }
            }

            order.StatusId = 7; // "Hủy đơn hàng"

            var orderStatusHistory = new OrderStatusHistory
            {
                OrderId = orderId,
                UserId = order.UserId,
                StatusId = 7,
                UpdatedDate = DateTime.Now,
                Note = reason
            };

            _context.OrderStatusHistories.Add(orderStatusHistory);

            await _context.SaveChangesAsync();
        }

        public async Task AdminCancelOrderAsync(int orderId, string reason)
        {
            var order = await _context.Orders
                .Include(o => o.OrderStatusHistories)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            if (order.StatusId != 1)
            {
                throw new InvalidOperationException("Chỉ có thể hủy đơn hàng khi trạng thái hiện tại là 'Chờ xác nhận'.");
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.ProductId.HasValue && orderDetail.Quantity.HasValue)
                {
                    await _productRepository.ReturnProductQuantity(orderDetail.ProductId.Value, orderDetail.Quantity.Value);
                }
            }

            order.StatusId = 3; // "Hủy xác nhận"

            var orderStatusHistory = new OrderStatusHistory
            {
                OrderId = orderId,
                UserId = order.UserId,
                StatusId = 3,
                UpdatedDate = DateTime.Now,
                Note = reason
            };

            _context.OrderStatusHistories.Add(orderStatusHistory);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.OrderStatusHistories)
                    .ThenInclude(osh => osh.Status)
                .ToListAsync();

            return orders.Select(o => new Order
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
                    Price = od.Price,
                    Quantity = od.Quantity
                }).ToList(),
                OrderStatusHistories = o.OrderStatusHistories.Select(osh => new OrderStatusHistory
                {
                    OrderId = osh.OrderId,
                    UserId = osh.UserId,
                    StatusId = osh.StatusId,
                    UpdatedDate = osh.UpdatedDate,
                    Note = osh.Note,
                    Status = osh.Status != null ? new OrderStatus
                    {
                        StatusId = osh.Status.StatusId,
                        StatusName = osh.Status.StatusName
                    } : null
                }).ToList(),
                StatusId = o.StatusId,
                PointsUsed = o.PointsUsed,
                Status = o.Status,
                CumulativeScoreTransactions = o.CumulativeScoreTransactions,
                DeliveryMethods = o.DeliveryMethods,
                Payments = o.Payments,
                Statistics = o.Statistics,
                User = o.User,
                Voucher = o.Voucher
            }).ToList();
        }

        //public async Task<OrderStatus> GetLatestOrderStatusAsync(int orderId)
        //{
        //    var order = await _context.Orders
        //        .Include(o => o.OrderStatuses)
        //        .FirstOrDefaultAsync(o => o.OrderId == orderId);

        //    if (order == null)
        //    {
        //        throw new ArgumentException("ID đơn hàng không hợp lệ.");
        //    }

        //    var latestStatus = order.OrderStatuses
        //        .OrderByDescending(os => os.StatusUpdateDate)
        //        .FirstOrDefault();

        //    if (latestStatus == null)
        //    {
        //        throw new InvalidOperationException("Không tìm thấy trạng thái đơn hàng.");
        //    }

        //    return latestStatus;
        //}

        public async Task<OrderStatusHistory?> GetLatestOrderStatusAsync(int orderId)
        {
            var latestStatus = await _context.OrderStatusHistories
                .Where(osh => osh.OrderId == orderId)
                .OrderByDescending(osh => osh.UpdatedDate)
                .FirstOrDefaultAsync();

            return latestStatus;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Include(o => o.OrderStatusHistories).ThenInclude(osh => osh.Status)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentException("ID đơn hàng không hợp lệ.");
            }

            var orderModel = new Order
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
                OrderDetails = order.OrderDetails.Select(od => new OrderDetail
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Price = od.Price,
                    Quantity = od.Quantity,
                    Product = new Product
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
                OrderStatusHistories = order.OrderStatusHistories.Select(osh => new OrderStatusHistory
                {
                    StatusId = osh.StatusId,
                    OrderId = osh.OrderId,
                    UserId = osh.UserId,
                    UpdatedDate = osh.UpdatedDate,
                    Note = osh.Note,
                    Status = osh.Status != null ? new OrderStatus
                    {
                        StatusId = osh.Status.StatusId,
                        StatusName = osh.Status.StatusName
                    } : null
                }).ToList()
            };

            return orderModel;
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Include(o => o.OrderStatusHistories).ThenInclude(osh => osh.Status)
                .ToListAsync();

            if (!orders.Any())
            {
                throw new ArgumentException("Không tìm thấy đơn hàng cho người dùng này.");
            }

            var orderModels = orders.Select(order => new Order
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
                OrderDetails = order.OrderDetails.Select(od => new OrderDetail
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    Product = od.Product != null ? new Product
                    {
                        ProductId = od.Product.ProductId,
                        ProductName = od.Product.ProductName,
                        Description = od.Product.Description,
                        Quantity = od.Product.Quantity,
                        IsSoldOut = od.Product.IsSoldOut,
                        BackInStockDate = od.Product.BackInStockDate,
                        CategoryId = od.Product.CategoryId,
                        BrandId = od.Product.BrandId,
                        FeedbackTotal = od.Product.FeedbackTotal,
                        OldPrice = od.Product.OldPrice,
                        Discount = od.Product.Discount,
                        NewPrice = od.Product.NewPrice,
                        ImageLinks = od.Product.ImageLinks,
                        Brand = od.Product.Brand,
                        Category = od.Product.Category
                    } : null
                }).ToList(),
                OrderStatusHistories = order.OrderStatusHistories.Select(osh => new OrderStatusHistory
                {
                    StatusId = osh.StatusId,
                    OrderId = osh.OrderId,
                    UserId = osh.UserId,
                    UpdatedDate = osh.UpdatedDate,
                    Note = osh.Note,
                    Status = osh.Status != null ? new OrderStatus
                    {
                        StatusId = osh.Status.StatusId,
                        StatusName = osh.Status.StatusName
                    } : null
                }).ToList()
            }).ToList();

            return orderModels;
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }


    }
}
