using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers.BlogCategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoryController : ControllerBase
    {
        private readonly BlogCategoryService _blogCategoryService;

        public BlogCategoryController(BlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpPost("AddBlogCategory")]
        public async Task<IActionResult> AddBlogCategory(string categoryName, int? parentCategoryId)
        {
            await _blogCategoryService.AddBlogCategory(categoryName, parentCategoryId);
            return Ok("Đã thêm danh mục cho Blog thành công");
        }

        [HttpDelete("DeleteBlogCategory/{categoryId}")]
        public async Task<IActionResult> DeleteBlogCategory(int categoryId)
        {
            await _blogCategoryService.DeleteBlogCategory(categoryId);
            return Ok("Đã xóa danh mục blog thành công");
        }

        [HttpPut("UpdateBlogCategory/{categoryId}")]
        public async Task<IActionResult> UpdateBlogCategory(int categoryId, string? categoryName, int? parentCategoryId)
        {
            await _blogCategoryService.UpdateBlogCategory(categoryId, categoryName, parentCategoryId);
            return Ok("Đã update danh mục blog thành công");
        }

        [HttpGet("GetBlogCategories")]
        public async Task<ActionResult<List<BlogCategory>>> GetAllBlogCategories()
        {
            var blogCategories = await _blogCategoryService.GetAllBlogCategories();
            return Ok(blogCategories);
        }

        [HttpGet("GetBlogCategoryById/{categoryId}")]
        public async Task<ActionResult<BlogCategory?>> GetBlogCategoryById(int categoryId)
        {
            var blogCategory = await _blogCategoryService.GetBlogCategoryById(categoryId);
            if (blogCategory == null)
            {
                return NotFound();
            }
            return Ok(blogCategory);
        }
    }
}
