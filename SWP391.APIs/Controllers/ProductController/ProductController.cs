using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.ProductServices;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(
            string productName,
            bool? isSelling,
            string description,
            int quantity,
            int isSoldOut,
            DateTime? backInStockDate,
            int? categoryId,
            int? brandId,
            int? feedbackTotal,
            int oldPrice,
            decimal discount,
            string imageLinks)
        {
            try
            {
                await _productService.AddProduct(
                    productName,
                    isSelling,
                    description,
                    quantity,
                    isSoldOut,
                    backInStockDate,
                    categoryId,
                    brandId,
                    feedbackTotal,
                    oldPrice,
                    discount,
                    imageLinks);

                return Ok(new { message = "Sản phẩm đã được thêm thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Thêm sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                await _productService.DeleteProduct(productId);
                return Ok(new { message = "Sản phẩm đã được xóa thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Xóa sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpGet("search-by-name/{name}")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            try
            {
                List<Product> foundProducts = await _productService.SearchProductByName(name);
                return Ok(foundProducts);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Tìm kiếm sản phẩm theo tên thất bại: {ex.Message}" });
            }
        }

        [HttpGet("search-by-status/{isSelling}")]
        public async Task<IActionResult> SearchProductByStatus(bool isSelling = true)
        {
            try
            {
                List<Product> foundProducts = await _productService.SearchProductByStatus(isSelling);
                return Ok(foundProducts);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Tìm kiếm sản phẩm theo trạng thái thất bại: {ex.Message}" });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> ShowAllProducts()
        {
            try
            {
                List<Product> allProducts = await _productService.ShowAllProducts();
                return Ok(allProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lấy danh sách sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpGet("sort-by-name")]
        public async Task<IActionResult> SortProductByName(bool ascending = true)
        {
            try
            {
                List<Product> sortedProducts = await _productService.SortProductByName(ascending);
                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sắp xếp sản phẩm theo tên thất bại: {ex.Message}" });
            }
        }

        [HttpGet("sort-by-price")]
        public async Task<IActionResult> SortProductByPrice(bool ascending = true)
        {
            try
            {
                List<Product> sortedProducts = await _productService.SortProductByPrice(ascending);
                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sắp xếp sản phẩm theo giá thất bại: {ex.Message}" });
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Dictionary<string, object> updates)
        {
            try
            {
                await _productService.UpdateProduct(productId, updates);
                return Ok(new { message = "Cập nhật sản phẩm thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Cập nhật sản phẩm thất bại: {ex.Message}" });
            }
        }
    }
}
