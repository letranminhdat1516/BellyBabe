using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Entities;
using SWP391.BLL.Services.CartServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCartAsync(int userId, int productId, int quantity)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("ID người dùng không hợp lệ.");
                }

                await _cartService.AddProductToCartAsync(userId, productId, quantity);
                return Ok("Đã thêm sản phẩm vào giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Thêm sản phẩm vào giỏ hàng thất bại: {ex.Message}");
            }
        }

        [HttpGet("cartdetails/{userId}")]
        public async Task<IActionResult> GetCartDetailsAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("ID người dùng không hợp lệ.");
                }

                var (orderDetails, message) = await _cartService.GetCartDetailsAsync(userId);
                if (orderDetails == null || !orderDetails.Any())
                {
                    return NotFound("Không có sản phẩm nào trong giỏ hàng.");
                }
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lấy thông tin giỏ hàng thất bại: {ex.Message}");
            }
        }

        [HttpPost("increaseQuantity")]
        public async Task<IActionResult> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("ID người dùng không hợp lệ.");
                }

                if (productId <= 0)
                {
                    return BadRequest("ID sản phẩm không hợp lệ.");
                }

                if (quantityToAdd <= 0)
                {
                    return BadRequest("Số lượng sản phẩm phải lớn hơn 0.");
                }

                await _cartService.IncreaseQuantityAsync(userId, productId, quantityToAdd);
                return Ok("Đã cập nhật số lượng sản phẩm trong giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}");
            }
        }

        [HttpPost("decreaseQuantity")]
        public async Task<IActionResult> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("ID người dùng không hợp lệ.");
                }

                if (productId <= 0)
                {
                    return BadRequest("ID sản phẩm không hợp lệ.");
                }

                if (quantityToSubtract <= 0)
                {
                    return BadRequest("Số lượng sản phẩm phải lớn hơn 0.");
                }

                await _cartService.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
                return Ok("Đã cập nhật số lượng sản phẩm trong giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}");
            }
        }

        [HttpDelete("deleteProductfromCart")]
        public async Task<IActionResult> DeleteProductFromCartAsync(int userId, int productId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("ID người dùng không hợp lệ.");
                }

                if (productId <= 0)
                {
                    return BadRequest("ID sản phẩm không hợp lệ.");
                }

                await _cartService.DeleteProductFromCartAsync(userId, productId);
                return Ok("Đã xóa sản phẩm khỏi giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Xóa sản phẩm khỏi giỏ hàng thất bại: {ex.Message}");
            }
        }
    }
}
