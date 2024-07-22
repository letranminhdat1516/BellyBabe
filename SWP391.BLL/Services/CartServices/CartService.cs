using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories;

namespace SWP391.BLL.Services.CartServices
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;

        public CartService(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<(List<OrderDetail>, string)> GetCartDetailsAsync(int userId)
        {
            try
            {
                var orderDetails = await _cartRepository.GetCartDetailsAsync(userId);
                return (orderDetails, "Đã lấy thông tin giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return (null, $"Lấy chi tiết giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> AddToCartAsync(int userId, int productId, int quantity, bool isChecked = false)
        {
            try
            {
                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                if(product.IsSoldOut == 1)
                {
                    return "Sản phẩm không thể thêm vào giỏ hàng.";
                }

                await _cartRepository.AddToCartAsync(userId, productId, quantity, isChecked);
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
                await _cartRepository.PurchaseNowAsync(userId, productId, quantity);
                return "Đã thêm sản phẩm vào giỏ hàng và đánh dấu để mua ngay.";
            }
            catch (Exception ex)
            {
                return $"Thêm sản phẩm để mua ngay thất bại: {ex.Message}";
            }
        }

        public async Task<string> UpdateIsCheckedAsync(int userId, int productId, bool isChecked)
        {
            try
            {
                await _cartRepository.UpdateIsCheckedAsync(userId, productId, isChecked);
                return "Đã cập nhật trạng thái chọn của sản phẩm trong giỏ hàng.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật trạng thái chọn thất bại: {ex.Message}";
            }
        }

        public async Task<string> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            try
            {
                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);
                if (orderDetail == null)
                {
                    return "Không tìm thấy sản phẩm trong giỏ hàng.";
                }

                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                if (product.Quantity < orderDetail.Quantity + quantityToAdd)
                {
                    return "Không đủ số lượng sản phẩm để thêm vào giỏ hàng.";
                }

                orderDetail.Quantity += quantityToAdd;
                orderDetail.Price = (int)(product.NewPrice * orderDetail.Quantity);

                await _cartRepository.UpdateOrderDetailAsync(orderDetail);

                return "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công.";
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
                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);
                if (orderDetail == null)
                {
                    return "Không tìm thấy sản phẩm trong giỏ hàng.";
                }

                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                orderDetail.Quantity -= quantityToSubtract;
                if (orderDetail.Quantity <= 0)
                {
                    await _cartRepository.RemoveOrderDetailAsync(orderDetail);
                    return "Sản phẩm đã được xóa khỏi giỏ hàng do số lượng bằng 0.";
                }
                else
                {
                    orderDetail.Price = (int)(product.NewPrice * orderDetail.Quantity);
                    await _cartRepository.UpdateOrderDetailAsync(orderDetail);
                }

                return "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công.";
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
                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);
                if (orderDetail == null)
                {
                    return "Không tìm thấy sản phẩm trong giỏ hàng.";
                }

                await _cartRepository.RemoveOrderDetailAsync(orderDetail);

                return "Sản phẩm đã được xóa khỏi giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                return $"Xóa sản phẩm khỏi giỏ hàng thất bại: {ex.Message}";
            }
        }
    }
}
