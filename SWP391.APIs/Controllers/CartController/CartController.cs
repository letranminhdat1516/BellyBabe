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
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            await _cartService.AddProductToCartAsync(userId, productId, quantity);
            return Ok("Product added to cart successfully.");
        }

        [HttpGet("cartdetails/{userId}")]
        public async Task<ActionResult<List<OrderDetail>>> GetCartDetailsAsync(int userId)
        {
            var orderDetails = await _cartService.GetCartDetailsAsync(userId);
            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound("No items in the cart.");
            }
            return Ok(orderDetails);
        }

        [HttpPost("increaseQuantity")]
        public async Task<IActionResult> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            if (productId <= 0)
            {
                return BadRequest("Product ID must be provided.");
            }

            if (quantityToAdd <= 0)
            {
                return BadRequest("Quantity to add must be greater than zero.");
            }

            await _cartService.IncreaseQuantityAsync(userId, productId, quantityToAdd);
            return Ok("Product quantity increased successfully.");
        }

        [HttpPost("decreaseQuantity")]
        public async Task<IActionResult> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            if (productId <= 0)
            {
                return BadRequest("Product ID must be provided.");
            }

            if (quantityToSubtract <= 0)
            {
                return BadRequest("Quantity to subtract must be greater than zero.");
            }

            await _cartService.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
            return Ok("Product quantity decreased successfully.");
        }

        [HttpDelete("deleteProductfromCart")]
        public async Task<IActionResult> DeleteProductFromCartAsync(int userId, int productId)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            if (productId <= 0)
            {
                return BadRequest("Product ID must be provided.");
            }

            await _cartService.DeleteProductFromCartAsync(userId, productId);
            return Ok("Product removed from cart successfully.");
        }
    }
}
