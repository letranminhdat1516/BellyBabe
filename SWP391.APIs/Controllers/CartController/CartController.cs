using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Entities;
using SWP391.BLL.Services.CartServices;
using System.Collections.Generic;
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

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCartAsync(int userId, int productId, int quantity, bool isChecked = false)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0)
            {
                return BadRequest("Thông số đầu vào không hợp lệ.");
            }

            var result = await _cartService.AddToCartAsync(userId, productId, quantity, isChecked);
            if (result.StartsWith("Thêm sản phẩm vào giỏ hàng thất bại"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("PurchaseNow")]
        public async Task<IActionResult> PurchaseNowAsync(int userId, int productId, int quantity)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0)
            {
                return BadRequest("Thông số đầu vào không hợp lệ.");
            }

            var result = await _cartService.PurchaseNowAsync(userId, productId, quantity);
            if (result.StartsWith("Thêm sản phẩm để mua ngay thất bại"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("UpdateIsChecked")]
        public async Task<IActionResult> UpdateIsCheckedAsync(int userId, int productId, bool isChecked)
        {
            if (userId <= 0 || productId <= 0)
            {
                return BadRequest("Thông số đầu vào không hợp lệ.");
            }

            var result = await _cartService.UpdateIsCheckedAsync(userId, productId, isChecked);
            if (result.StartsWith("Cập nhật trạng thái chọn thất bại"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("CartDetails/{userId}")]
        public async Task<ActionResult<List<OrderDetail>>> GetCartDetailsAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("ID người dùng không hợp lệ.");
            }

            var (orderDetails, message) = await _cartService.GetCartDetailsAsync(userId);
            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NotFound(message);
            }

            return Ok(orderDetails);
        }

        [HttpPost("IncreaseQuantity")]
        public async Task<IActionResult> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            if (userId <= 0 || productId <= 0 || quantityToAdd <= 0)
            {
                return BadRequest("Thông số đầu vào không hợp lệ.");
            }

            var result = await _cartService.IncreaseQuantityAsync(userId, productId, quantityToAdd);
            if (result.StartsWith("Cập nhật số lượng sản phẩm thất bại"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("DecreaseQuantity")]
        public async Task<IActionResult> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            if (userId <= 0 || productId <= 0 || quantityToSubtract <= 0)
            {
                return BadRequest("Thông số đầu vào không hợp lệ.");
            }

            var result = await _cartService.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
            if (result.StartsWith("Cập nhật số lượng sản phẩm thất bại"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("DeleteProductFromCart")]
        public async Task<IActionResult> DeleteProductFromCartAsync(int userId, int productId)
        {
            if (userId <= 0 || productId <= 0)
            {
                return BadRequest("Thông số đầu vào không hợp lệ.");
            }

            var result = await _cartService.DeleteProductFromCartAsync(userId, productId);
            if (result.StartsWith("Xóa sản phẩm khỏi giỏ hàng thất bại"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
