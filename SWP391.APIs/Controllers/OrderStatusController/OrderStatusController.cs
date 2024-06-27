using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.OrderStatusServices;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        private readonly OrderStatusService _orderStatusService;

        public OrderStatusController(OrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [HttpGet("GetAllOrderStatuses")]
        public async Task<IActionResult> GetAllOrderStatuses()
        {
            try
            {
                List<OrderStatus> orderStatuses = await _orderStatusService.GetAllOrderStatuses();
                return Ok(orderStatuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy danh sách trạng thái đơn hàng: {ex.Message}" });
            }
        }

        [HttpGet("GetOrderStatusById/{statusId}")]
        public async Task<IActionResult> GetOrderStatusById(int statusId)
        {
            try
            {
                OrderStatus orderStatus = await _orderStatusService.GetOrderStatusById(statusId);
                return Ok(orderStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy thông tin trạng thái đơn hàng: {ex.Message}" });
            }
        }

        [HttpPost("AddOrderStatus")]
        public async Task<IActionResult> AddOrderStatus(OrderStatus orderStatus)
        {
            try
            {
                await _orderStatusService.AddOrderStatus(orderStatus);
                return Ok(new { message = "Thêm trạng thái đơn hàng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm trạng thái đơn hàng: {ex.Message}" });
            }
        }

        [HttpPut("UpdateOrderStatus/{orderStatus}")]
        public async Task<IActionResult> UpdateOrderStatus(int statusId, string statusName)
        {
            try
            {
                await _orderStatusService.UpdateOrderStatus(statusId, statusName);
                return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi cập nhật trạng thái đơn hàng: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteOrderStatus/{statusId}")]
        public async Task<IActionResult> DeleteOrderStatus(int statusId)
        {
            try
            {
                await _orderStatusService.DeleteOrderStatus(statusId);
                return Ok(new { message = "Xóa trạng thái đơn hàng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa trạng thái đơn hàng: {ex.Message}" });
            }
        }
    }
}
