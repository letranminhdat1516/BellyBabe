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

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCartAsync([FromQuery] int userId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            if (userId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID người dùng.");
            }

            var result = await _cartService.AddProductToCartAsync(userId, productId, quantity);
            if (result.Contains("thất bại"))
            {
                return BadRequest(result);
            }

            return Ok("Đã thêm sản phẩm vào giỏ hàng thành công.");
        }

        [HttpGet("cart-details/{userId}")]
        public async Task<ActionResult<List<OrderDetail>>> GetCartDetailsAsync(int userId)
        {
            var (orderDetails, message) = await _cartService.GetCartDetailsAsync(userId);
            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NotFound("Không có sản phẩm nào trong giỏ hàng.");
            }

            return Ok(orderDetails);
        }

        [HttpPost("increase-quantity")]
        public async Task<IActionResult> IncreaseQuantityAsync([FromQuery] int userId, [FromQuery] int productId, [FromQuery] int quantityToAdd)
        {
            if (userId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID người dùng.");
            }

            if (productId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID sản phẩm.");
            }

            if (quantityToAdd <= 0)
            {
                return BadRequest("Số lượng cần thêm phải lớn hơn không.");
            }

            var result = await _cartService.IncreaseQuantityAsync(userId, productId, quantityToAdd);
            if (result.Contains("thất bại"))
            {
                return BadRequest(result);
            }

            return Ok("Đã tăng số lượng sản phẩm trong giỏ hàng thành công.");
        }

        [HttpPost("decrease-quantity")]
        public async Task<IActionResult> DecreaseQuantityAsync([FromQuery] int userId, [FromQuery] int productId, [FromQuery] int quantityToSubtract)
        {
            if (userId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID người dùng.");
            }

            if (productId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID sản phẩm.");
            }

            if (quantityToSubtract <= 0)
            {
                return BadRequest("Số lượng cần giảm phải lớn hơn không.");
            }

            var result = await _cartService.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
            if (result.Contains("thất bại"))
            {
                return BadRequest(result);
            }

            return Ok("Đã giảm số lượng sản phẩm trong giỏ hàng thành công.");
        }

        [HttpDelete("delete-product-from-cart")]
        public async Task<IActionResult> DeleteProductFromCartAsync([FromQuery] int userId, [FromQuery] int productId)
        {
            if (userId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID người dùng.");
            }

            if (productId <= 0)
            {
                return BadRequest("Vui lòng cung cấp ID sản phẩm.");
            }

            var result = await _cartService.DeleteProductFromCartAsync(userId, productId);
            if (result.Contains("thất bại"))
            {
                return BadRequest(result);
            }

            return Ok("Đã xóa sản phẩm khỏi giỏ hàng thành công.");
        }
    }
}
