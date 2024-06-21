using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerController(ManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpPost("AddManufacturer")]
        public async Task<IActionResult> AddManufacturer(string manufacturerName)
        {
            await _manufacturerService.AddManufacturer(manufacturerName);
            return Ok("Add Manufacturer successfully");
        }

        [HttpDelete("DeleteManufacturer/{manufacturerId}")]
        public async Task<IActionResult> DeleteManufacturer(int manufacturerId)
        {
            await _manufacturerService.DeleteManufacturer(manufacturerId);
            return Ok();
        }

        [HttpPut("UpdateManufacturer/{manufacturerId}")]
        public async Task<IActionResult> UpdateManufacturer(int manufacturerId, [FromBody] Dictionary<string, object> updates)
        {
            await _manufacturerService.UpdateManufacturer(manufacturerId, updates);
            return Ok();
        }

        [HttpGet("GetManufacturers")]
        public async Task<ActionResult<List<Manufacturer>>> GetAllManufacturers()
        {
            var manufacturers = await _manufacturerService.GetAllManufacturers();
            return Ok(manufacturers);
        }

        [HttpGet("GetManufacturerById/{manufacturerId}")]
        public async Task<ActionResult<Manufacturer?>> GetManufacturerById(int manufacturerId)
        {
            var manufacturer = await _manufacturerService.GetManufacturerById(manufacturerId);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return Ok(manufacturer);
        }
    }
}
