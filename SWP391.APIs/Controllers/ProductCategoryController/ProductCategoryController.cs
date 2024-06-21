using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoryController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpPost("AddProductCategory")]
        public async Task<IActionResult> AddProductCategory(string categoryName, int? parentCategoryId)
        {
            await _productCategoryService.AddProductCategory(categoryName, parentCategoryId);
            return Ok("Add Product Category successfully");
        }

        [HttpDelete("DeleteProductCategory/{categoryId}")]
        public async Task<IActionResult> DeleteProductCategory(int categoryId)
        {
            await _productCategoryService.DeleteProductCategory(categoryId);
            return Ok();
        }

        [HttpPut("UpdateProductCategory/{categoryId}")]
        public async Task<IActionResult> UpdateProductCategory(int categoryId, [FromBody] Dictionary<string, object> updates)
        {
            await _productCategoryService.UpdateProductCategory(categoryId, updates);
            return Ok();
        }

        [HttpGet("GetAllProductCategories")]
        public async Task<ActionResult<List<ProductCategory>>> GetAllProductCategories()
        {
            var productCategories = await _productCategoryService.GetAllProductCategories();
            return Ok(productCategories);
        }

        [HttpGet("GetProductCategoryById/{categoryId}")]
        public async Task<ActionResult<ProductCategory?>> GetProductCategoryById(int categoryId)
        {
            var productCategory = await _productCategoryService.GetProductCategoryById(categoryId);
            if (productCategory == null)
            {
                return NotFound();
            }
            return Ok(productCategory);
        }
    }
}
