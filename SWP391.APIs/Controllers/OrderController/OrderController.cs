using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Entities;
using SWP391.BLL.Services.OrderServices;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string paymentMethod, string note)
        {
            if (userId <= 0)
            {
                return BadRequest("ID người dùng phải được cung cấp.");
            }

            await _orderService.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, deliveryId, paymentMethod, note);
            return Ok("Đặt hàng thành công.");
        }

        [HttpGet("orders/{userId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersAsync(int userId)
        {
            var orders = await _orderService.GetOrdersAsync(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound("Không tìm thấy đơn hàng nào cho người dùng này.");
            }
            return Ok(orders);
        }

        [HttpPost("updateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            if (orderId <= 0)
            {
                return BadRequest("ID đơn hàng phải được cung cấp.");
            }

            await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return Ok("Cập nhật trạng thái đơn hàng thành công.");
        }

        [HttpGet("ordersByStatus/{userId}/{statusName}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByStatusAsync(int userId, string statusName)
        {
            if (userId <= 0)
            {
                return BadRequest("ID người dùng phải được cung cấp.");
            }

            var orders = await _orderService.GetOrdersByStatusAsync(userId, statusName);
            if (orders == null || !orders.Any())
            {
                return NotFound($"Không tìm thấy đơn hàng nào cho người dùng với trạng thái {statusName}.");
            }
            return Ok(orders);
        }

        [HttpPost("cancelOrder")]
        public async Task<IActionResult> CancelOrderAsync(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("ID đơn hàng phải được cung cấp.");
            }

            await _orderService.CancelOrderAsync(orderId);
            return Ok("Hủy đơn hàng thành công.");
        }
    }
}
