using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;

namespace SWP391.DAL.Repositories
{
    public class CartRepository
    {
        private readonly Swp391Context _context;

        public CartRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<string> AddToCartAsync(int userId, int productId, int quantity, bool isChecked = false)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new ArgumentException("ID sản phẩm không hợp lệ.");
                }

                if (product.IsSelling != true)
                {
                    throw new Exception("Sản phẩm hiện không có sẵn để mua.");
                }

                if (quantity <= 0)
                {
                    throw new ArgumentException("Số lượng sản phẩm phải lớn hơn 0.");
                }

                var orderDetail = await _context.OrderDetails
                    .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

                if (orderDetail == null)
                {
                    orderDetail = new OrderDetail
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        Price = (int)(product.NewPrice * quantity),
                        IsChecked = isChecked
                    };

                    _context.OrderDetails.Add(orderDetail);
                }
                else
                {
                    orderDetail.Quantity += quantity;
                    orderDetail.IsChecked = isChecked;
                }

                await _context.SaveChangesAsync();

                return "Đã thêm sản phẩm vào giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                return $"Thêm sản phẩm vào giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> PurchaseNowAsync(int userId, int productId, int quantity)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new ArgumentException("ID sản phẩm không hợp lệ.");
                }

                if (product.IsSelling != true)
                {
                    throw new Exception("Sản phẩm hiện không có sẵn để mua.");
                }

                if (quantity <= 0)
                {
                    throw new ArgumentException("Số lượng sản phẩm phải lớn hơn 0.");
                }

                var orderDetail = new OrderDetail
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = (int)(product.NewPrice * quantity),
                    IsChecked = true
                };

                _context.OrderDetails.Add(orderDetail);

                await _context.SaveChangesAsync();

                return "Đã mua ngay sản phẩm thành công.";
            }
            catch (Exception ex)
            {
                return $"Mua ngay sản phẩm thất bại: {ex.Message}";
            }
        }

        public async Task<(List<OrderDetail>, string)> GetCartDetailsAsync(int userId)
        {
            try
            {
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.UserId == userId && od.OrderId == null)
                    .Include(od => od.Product)
                    .ToListAsync();

                return (orderDetails, "Đã lấy thông tin giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return (null, $"Lấy thông tin giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            try
            {
                var orderDetail = await _context.OrderDetails
                    .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

                if (orderDetail != null)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product == null)
                    {
                        throw new ArgumentException("ID sản phẩm không hợp lệ.");
                    }

                    if (product.IsSelling != true)
                    {
                        throw new Exception("Sản phẩm hiện không có sẵn để mua.");
                    }

                    var availableQuantity = product.Quantity;

                    if (availableQuantity < orderDetail.Quantity + quantityToAdd)
                    {
                        throw new Exception("Không đủ số lượng sản phẩm để thêm vào giỏ hàng.");
                    }

                    // Calculate the new price based on the updated quantity
                    var pricePerItem = product.NewPrice;
                    var totalPrice = pricePerItem * (orderDetail.Quantity + quantityToAdd);

                    orderDetail.Quantity += quantityToAdd;
                    orderDetail.Price = (int)totalPrice;

                    await _context.SaveChangesAsync();

                    return "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công.";
                }

                return "Không tìm thấy sản phẩm trong giỏ hàng.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            try
            {
                var orderDetail = await _context.OrderDetails
                    .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

                if (orderDetail != null)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product == null)
                    {
                        throw new ArgumentException("ID sản phẩm không hợp lệ.");
                    }

                    if (product.IsSelling != true)
                    {
                        throw new Exception("Sản phẩm hiện không có sẵn để mua.");
                    }

                    orderDetail.Quantity -= quantityToSubtract;
                    if (orderDetail.Quantity <= 0)
                    {
                        _context.OrderDetails.Remove(orderDetail);
                    }
                    else
                    {
                        // Calculate the new price based on the updated quantity
                        var pricePerItem = product.NewPrice;
                        var totalPrice = pricePerItem * orderDetail.Quantity;

                        orderDetail.Price = (int)totalPrice;
                    }

                    await _context.SaveChangesAsync();

                    return "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công.";
                }

                return "Không tìm thấy sản phẩm trong giỏ hàng.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> DeleteProductFromCartAsync(int userId, int productId)
        {
            try
            {
                var orderDetail = await _context.OrderDetails
                    .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

                if (orderDetail != null)
                {
                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();

                    return "Sản phẩm đã được xóa khỏi giỏ hàng thành công.";
                }

                return "Không tìm thấy sản phẩm trong giỏ hàng.";
            }
            catch (Exception ex)
            {
                return $"Xóa sản phẩm khỏi giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> UpdateProductCheckedStatusAsync(int userId, int orderDetailId, bool isChecked)
        {
            try
            {
                var orderDetail = await _context.OrderDetails
                    .FirstOrDefaultAsync(od => od.UserId == userId && od.OrderDetailId == orderDetailId && od.OrderId == null);

                if (orderDetail != null)
                {
                    orderDetail.IsChecked = isChecked;
                    await _context.SaveChangesAsync();

                    return "Cập nhật trạng thái sản phẩm thành công.";
                }

                return "Không tìm thấy chi tiết đơn hàng trong giỏ hàng.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật trạng thái sản phẩm thất bại: {ex.Message}";
            }
        }
    }
}
