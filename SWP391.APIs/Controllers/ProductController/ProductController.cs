﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(string productName, bool? isSelling, string? description, int quantity, int isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int? oldPrice, decimal? discount, string? imageLinks)
        {
            try
            {
                await _productService.AddProduct(productName, isSelling, description, quantity, isSoldOut, backInStockDate, categoryId, brandId, feedbackTotal, oldPrice, discount, imageLinks);
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
                List<Product> foundProducts = await _productService.SearchProductByName(name);
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
                List<Product> foundProducts = await _productService.SearchProductByStatus(isSelling);
                return Ok(foundProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Tìm kiếm sản phẩm theo trạng thái thất bại: {ex.Message}" });
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> ShowAllProducts()
        {
            try
            {
                List<Product> allProducts = await _productService.ShowAllProducts();
                return Ok(allProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lấy danh sách tất cả sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpGet("SortProductByName")]
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

        [HttpGet("SortProductByPrice")]
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

        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, string productName, bool? isSelling, string? description, int quantity, int isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int? oldPrice, decimal? discount, string? imageLinks)
        {
            try
            {
                await _productService.UpdateProduct(productId, productName, isSelling, description, quantity, isSoldOut, backInStockDate, categoryId, brandId, feedbackTotal, oldPrice, discount, imageLinks);
                return Ok(new { message = "Cập nhật sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Cập nhật sản phẩm thất bại: {ex.Message}" });
            }
        }

        [HttpPut("UpdateProducQuantity/{productId}")]
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