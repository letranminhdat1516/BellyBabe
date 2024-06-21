using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandService _brandService;

        public BrandController(BrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBrand(string brandName, string? description)
        {
            await _brandService.AddBrand(brandName, description);
            return Ok("Add Brand successfully");
        }

        [HttpDelete("DeleteBrand/{brandId}")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            await _brandService.DeleteBrand(brandId);
            return Ok();
        }

        [HttpPut("UpdateBrand/{brandId}")]
        public async Task<IActionResult> UpdateBrand(int brandId, [FromBody] Dictionary<string, object> updates)
        {
            await _brandService.UpdateBrand(brandId, updates);
            return Ok();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<List<Brand>>> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrands();
            return Ok(brands);
        }

        [HttpGet("GetBrandsById/{brandId}")]
        public async Task<ActionResult<Brand?>> GetBrandById(int brandId)
        {
            var brand = await _brandService.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }
    }
}
