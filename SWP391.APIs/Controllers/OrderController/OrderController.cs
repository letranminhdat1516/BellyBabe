using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.OrderServices;
using SWP391.DAL.Entities;
using System;
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
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string note)
        {
            try
            {
                await _orderService.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, deliveryId, note);
                return Ok(new { Message = "Đặt hàng thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = $"Lỗi đặt hàng: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("get-orders/{userId}")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync(userId);
                return Ok(orders);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = $"Lỗi lấy đơn hàng: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPut("update-order-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string statusName)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, statusName);
                return Ok(new { Message = "Cập nhật trạng thái đơn hàng thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = $"Lỗi cập nhật trạng thái đơn hàng: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("get-orders-by-status/{userId}")]
        public async Task<IActionResult> GetOrdersByStatus(int userId, string statusName)
        {
            try
            {
                var orders = await _orderService.GetOrdersByStatusAsync(userId, statusName);
                return Ok(orders);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = $"Lỗi lấy đơn hàng theo trạng thái: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpDelete("cancel-order/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId);
                return Ok(new { Message = "Hủy đơn hàng thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = $"Lỗi hủy đơn hàng: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}
