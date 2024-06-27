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
        public async Task<IActionResult> AddBrand(string brandName, string? description, string? imageBrand)
        {
            await _brandService.AddBrand(brandName, description, imageBrand);
            return Ok("Đã thêm thương hiệu thành công");
        }

        [HttpDelete("DeleteBrand/{brandId}")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            await _brandService.DeleteBrand(brandId);
            return Ok("Đã xóa thương hiệu thành công");
        }

        [HttpPut("UpdateBrand/{brandId}")]
        public async Task<IActionResult> UpdateBrand(int brandId, string? brandName, string? description, string? imageBrand)
        {
            await _brandService.UpdateBrand(brandId, brandName, description, imageBrand);
            return Ok("Đã cập nhật thương hiệu thành công");
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<List<Brand>>> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrands();
            return Ok(brands);
        }

        [HttpGet("GetBrandById/{brandId}")]
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
