using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
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
            _deliveryService = deliveryService;
        }

        [HttpPost("AddDelivery")]
        public async Task<IActionResult> AddDelivery(string deliveryName, string deliveryMethod, decimal deliveryFee)
        {
            await _deliveryService.AddDelivery(deliveryName, deliveryMethod, deliveryFee);
            return Ok("Add delivery successfully");
        }

        [HttpDelete("DeleteDelivery/{deliveryId}")]
        public async Task<IActionResult> DeleteDelivery(int deliveryId)
        {
            await _deliveryService.DeleteDelivery(deliveryId);
            return Ok();
        }

        [HttpPut("UpdateDelivery/{deliveryId}")]
        public async Task<IActionResult> UpdateDelivery(int deliveryId, [FromBody] Dictionary<string, object> updates)
        {
            await _deliveryService.UpdateDelivery(deliveryId, updates);
            return Ok();
        }

        [HttpGet("GetAllDeliveries")]
        public async Task<ActionResult<List<Delivery>>> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveries();
            return Ok(deliveries);
        }

        [HttpGet("GetDeliveriesById/{deliveryId}")]
        public async Task<ActionResult<Delivery?>> GetDeliveryById(int deliveryId)
        {
            var delivery = await _deliveryService.GetDeliveryById(deliveryId);
            if (delivery == null)
            {
                return NotFound();
            }
            return Ok(delivery);
        }
    }
}
