using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.CartRepository;

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
                var (orderDetails, message) = await _cartRepository.GetCartDetailsAsync(userId);
                return (orderDetails, message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy chi tiết giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> AddProductToCartAsync(int userId, int productId, int quantity)
        {
            try
            {
                var result = await _cartRepository.AddToCartAsync(userId, productId, quantity);
                return result;
            }
            catch (Exception ex)
            {
                return $"Thêm sản phẩm vào giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            try
            {
                var result = await _cartRepository.IncreaseQuantityAsync(userId, productId, quantityToAdd);
                return result;
            }
            catch (Exception ex)
            {
                return $"Cập nhật số lượng sản phẩm thất bại: {ex.Message}";
            }
        }

        public async Task<string> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            try
            {
                var result = await _cartRepository.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
                return result;
            }
            catch (Exception ex)
            {
                return $"Cập nhật số lượng sản phẩm thất bại: {ex.Message}";
            }
        }

        public async Task<string> DeleteProductFromCartAsync(int userId, int productId)
        {
            try
            {
                var result = await _cartRepository.DeleteProductFromCartAsync(userId, productId);
                return result;
            }
            catch (Exception ex)
            {
                return $"Xóa sản phẩm khỏi giỏ hàng thất bại: {ex.Message}";
            }
        }
    }
}
