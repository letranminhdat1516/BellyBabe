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

        public async Task<string> AddProductToCartAsync(int userId, int productId, int quantity)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                var result = await _cartRepository.AddToCartAsync(userId, productId, quantity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Thêm sản phẩm vào giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<(List<OrderDetail>, string)> GetCartDetailsAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                var result = await _cartRepository.GetCartDetailsAsync(userId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy thông tin giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                if (productId <= 0)
                {
                    throw new ArgumentException("ID sản phẩm không hợp lệ.");
                }

                if (quantityToAdd <= 0)
                {
                    throw new ArgumentException("Số lượng sản phẩm phải lớn hơn 0.");
                }

                var result = await _cartRepository.IncreaseQuantityAsync(userId, productId, quantityToAdd);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                if (productId <= 0)
                {
                    throw new ArgumentException("ID sản phẩm không hợp lệ.");
                }

                if (quantityToSubtract <= 0)
                {
                    throw new ArgumentException("Số lượng sản phẩm phải lớn hơn 0.");
                }

                var result = await _cartRepository.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> DeleteProductFromCartAsync(int userId, int productId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("ID người dùng không hợp lệ.");
                }

                if (productId <= 0)
                {
                    throw new ArgumentException("ID sản phẩm không hợp lệ.");
                }

                var result = await _cartRepository.DeleteProductFromCartAsync(userId, productId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Xóa sản phẩm khỏi giỏ hàng thất bại: {ex.Message}");
            }
        }
    }
}
