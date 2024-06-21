using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Entities;
using SWP391.BLL.Services.OrderServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrderAsync(int userId, string address, int deliveryId, string paymentMethod, string phoneNumber, string note)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            await _orderService.PlaceOrderAsync(userId, address, deliveryId, paymentMethod, phoneNumber, note);
            return Ok("Order placed successfully.");
        }

        [HttpGet("orders/{userId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersAsync(int userId)
        {
            var orders = await _orderService.GetOrdersAsync(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for this user.");
            }
            return Ok(orders);
        }

        [HttpPost("updateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            if (orderId <= 0)
            {
                return BadRequest("Order ID must be provided.");
            }

            await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return Ok("Order status updated successfully.");
        }

        [HttpGet("ordersByStatus/{userId}/{statusName}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByStatusAsync(int userId, string statusName)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            var orders = await _orderService.GetOrdersByStatusAsync(userId, statusName);
            if (orders == null || !orders.Any())
            {
                return NotFound($"No orders found for user with status {statusName}.");
            }
            return Ok(orders);
        }

        [HttpPost("cancelOrder")]
        public async Task<IActionResult> CancelOrderAsync(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("Order ID must be provided.");
            }

            await _orderService.CancelOrderAsync(orderId);
            return Ok("Order cancelled successfully.");
        }
    }
}
