﻿using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.ProductServices;
using SWP391.DAL.Model.Product;
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
        private readonly string _imageFolderPath = @"D:\Semester 05\SWP391\Project\swp391-project-fe\src\assets\images\products";

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages(int productId, [FromForm] List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest("No images received.");
            }

            try
            {
                var imageLinks = new List<string>();

                foreach (var image in images)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(_imageFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imageLinks.Add(fileName);
                }

                await _productService.UpdateProductImageLinksAsync(productId, imageLinks);
                return Ok(new { Message = "Images uploaded successfully.", ImageLinks = imageLinks });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(string productName, string? description, int quantity, int? categoryId, int? brandId, int? oldPrice, decimal? discount, string? imageLinks)
        {
            try
            {
                await _productService.AddProduct(productName, description, quantity, categoryId, brandId, oldPrice, discount, imageLinks);
                return Ok(new { message = "Thêm sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Thêm sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                await _productService.DeleteProduct(productId);
                return Ok(new { message = "Xóa sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Xóa sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpGet("SearchByName/{name}")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            try
            {
                List<ProductModel> foundProducts = await _productService.SearchProductByName(name);
                return Ok(foundProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Tìm kiếm sản phẩm theo tên thất bại: {ex.Message}" });
            }
        }

        [HttpGet("SearchProductByStatus/{isSelling}")]
        public async Task<IActionResult> SearchProductByStatus(bool isSelling)
        {
            try
            {
                List<ProductModel> foundProducts = await _productService.SearchProductByStatus(isSelling);
                return Ok(foundProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Tìm kiếm sản phẩm theo trạng thái thất bại: {ex.Message}" });
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<ProductModel>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.ShowAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lấy danh sách sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpGet("SortProductByName")]
        public async Task<IActionResult> SortProductByName(bool ascending = true)
        {
            try
            {
                List<ProductModel> sortedProducts = await _productService.SortProductByName(ascending);
                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sắp xếp sản phẩm theo tên thất bại: {ex.Message}" });
            }
        }

        [HttpGet("SortProductByPrice")]
        public async Task<IActionResult> SortProductByPrice(bool ascending = true)
        {
            try
            {
                List<ProductModel> sortedProducts = await _productService.SortProductByPrice(ascending);
                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sắp xếp sản phẩm theo giá thất bại: {ex.Message}" });
            }
        }

        [HttpGet("GetProductById/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                var product = await _productService.GetProductById(productId);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lấy sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, string? productName, string? description, int? quantity, int? categoryId, int? brandId, int? oldPrice, decimal? discount, string? imageLinks)
        {
            try
            {
                await _productService.UpdateProduct(productId, productName, description, quantity, categoryId, brandId, oldPrice, discount, imageLinks);
                return Ok(new { message = "Cập nhật sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Cập nhật sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpPut("UpdateProductQuantity/{productId}")]
        public async Task<IActionResult> UpdateProductQuantity(int productId, int quantity)
        {
            try
            {
                await _productService.UpdateProductQuantity(productId, quantity);
                return Ok(new { message = "Cập nhật số lượng sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Cập nhật số lượng sản phẩm thất bại: {ex.Message}" });
            }
        }
    }
}