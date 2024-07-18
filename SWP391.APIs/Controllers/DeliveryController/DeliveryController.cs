using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            _deliveryService = deliveryService ?? throw new ArgumentNullException(nameof(deliveryService));
        }

        [HttpPost("AddDelivery")]
        public async Task<IActionResult> AddDelivery([FromQuery] string deliveryName, [FromQuery] int? deliveryFee)
        {
            try
            {
                await _deliveryService.AddDelivery(deliveryName, deliveryFee);
                return Ok("Thêm phương thức giao hàng thành công.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpDelete("DeleteDelivery/{deliveryId}")]
        public async Task<IActionResult> DeleteDelivery([FromRoute] int deliveryId)
        {
            try
            {
                await _deliveryService.DeleteDelivery(deliveryId);
                return Ok("Xóa phương thức giao hàng thành công.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Không tìm thấy phương thức giao hàng để xóa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpPut("UpdateDelivery/{deliveryId}")]
        public async Task<IActionResult> UpdateDelivery([FromRoute] int deliveryId, [FromQuery] string? deliveryName, [FromQuery] int? deliveryFee)
        {
            try
            {
                await _deliveryService.UpdateDelivery(deliveryId, deliveryName, deliveryFee);
                return Ok("Cập nhật phương thức giao hàng thành công.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpGet("GetAllDeliveries")]
        public async Task<ActionResult<List<DeliveryMethod>>> GetAllDeliveries()
        {
            try
            {
                var deliveries = await _deliveryService.GetAllDeliveries();
                return Ok(deliveries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpGet("GetDeliveryById/{deliveryId}")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryById([FromRoute] int deliveryId)
        {
            try
            {
                var delivery = await _deliveryService.GetDeliveryById(deliveryId);
                if (delivery == null)
                {
                    return NotFound("Không tìm thấy phương thức giao hàng.");
                }
                return Ok(delivery);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Không tìm thấy phương thức giao hàng.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}
