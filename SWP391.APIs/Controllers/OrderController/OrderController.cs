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
            _orderService = orderService;
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string? note, bool? usePoints = null)
        {
            try
            {
                await _orderService.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, deliveryId, note, usePoints);
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

        [HttpGet("GetOrdersFromUser/{userId}")]
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

        [HttpPut("UpdateOrderStatus/{orderId}")]
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

        [HttpGet("GetOrdersByStatusFromUser/{userId}")]
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

        [HttpDelete("CancelOrder/{orderId}")]
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

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var listOfOrders = await _orderService.GetAllOrders();
            return Ok(listOfOrders);
        }

        [HttpGet("GetLatestOrderStatus/{orderId}")]
        public async Task<IActionResult> GetLatestOrderStatus(int orderId)
        {
            try
            {
                var latestStatus = await _orderService.GetLatestOrderStatusAsync(orderId);
                return Ok(latestStatus);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = $"Lỗi lấy trạng thái mới nhất của đơn hàng: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(order);
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
    }
}
