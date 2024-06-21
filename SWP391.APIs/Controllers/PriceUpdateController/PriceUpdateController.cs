using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceUpdateController : ControllerBase
    {
        private readonly PriceUpdateService _priceUpdateService;

        public PriceUpdateController(PriceUpdateService priceUpdateService)
        {
            _priceUpdateService = priceUpdateService;
        }

        [HttpPost("AddPriceUpdate")]
        public async Task<IActionResult> AddPriceUpdate(decimal? price, DateTime? dateApplied, int? productId)
        {
            await _priceUpdateService.AddPriceUpdate(price, dateApplied, productId);
            return Ok("Add Price successfully");
        }

        [HttpDelete("DeletePriceUpdate/{priceId}")]
        public async Task<IActionResult> DeletePriceUpdate(int priceId)
        {
            await _priceUpdateService.DeletePriceUpdate(priceId);
            return Ok();
        }

        [HttpPut("UpdatePriceUpdate/{priceId}")]
        public async Task<IActionResult> UpdatePriceUpdate(int priceId, [FromBody] Dictionary<string, object> updates)
        {
            await _priceUpdateService.UpdatePriceUpdate(priceId, updates);
            return Ok();
        }

        [HttpGet("GetAllPriceUpdates")]
        public async Task<ActionResult<List<PriceUpdate>>> GetAllPriceUpdates()
        {
            var priceUpdates = await _priceUpdateService.GetAllPriceUpdates();
            return Ok(priceUpdates);
        }

        [HttpGet("GetPriceUpdatesById/{priceId}")]
        public async Task<ActionResult<PriceUpdate?>> GetPriceUpdateById(int priceId)
        {
            var priceUpdate = await _priceUpdateService.GetPriceUpdateById(priceId);
            if (priceUpdate == null)
            {
                return NotFound();
            }
            return Ok(priceUpdate);
        }

        [HttpGet("GetPriceUpdatesByProduct/{productId}")]
        public async Task<ActionResult<List<PriceUpdate>>> GetPriceUpdatesByProductId(int productId)
        {
            var priceUpdates = await _priceUpdateService.GetPriceUpdatesByProductId(productId);
            return Ok(priceUpdates);
        }
    }
}
