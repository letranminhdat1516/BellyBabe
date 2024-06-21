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
        public async Task<IActionResult> AddProduct(string productName, bool isSelling, string madeIn, string unit, string suitableAge, string usageInstructions, string storageInstructions, int quantity, int categoryId, int brandId, int manufacturerId, int rating)
        {
            try
            {
                await _productService.AddProduct(productName, isSelling, madeIn, unit, suitableAge, usageInstructions, storageInstructions, quantity, categoryId, brandId, manufacturerId, rating);
                return Ok(new { message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to add product: {ex.Message}" });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                await _productService.DeleteProduct(productId);
                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to delete product: {ex.Message}" });
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to search products by name: {ex.Message}" });
            }
        }

        [HttpGet("search-by-status/{isSelling}")]
        public async Task<IActionResult> SearchProductByStatus(bool isSelling)
        {
            try
            {
                List<Product> foundProducts = await _productService.SearchProductByStatus(isSelling);
                return Ok(foundProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to search products by status: {ex.Message}" });
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
                return StatusCode(500, new { message = $"Failed to retrieve all products: {ex.Message}" });
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
                return StatusCode(500, new { message = $"Failed to sort products by name: {ex.Message}" });
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
                return StatusCode(500, new { message = $"Failed to sort products by price: {ex.Message}" });
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Dictionary<string, object> updates)
        {
            try
            {
                await _productService.UpdateProduct(productId, updates);
                return Ok(new { message = "Product updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to update product: {ex.Message}" });
            }
        }
    }
}
