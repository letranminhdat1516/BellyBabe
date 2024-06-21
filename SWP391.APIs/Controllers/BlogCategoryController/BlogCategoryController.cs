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

        [HttpPost("add-blog-category")]
        public async Task<IActionResult> AddBlogCategory(string categoryName, int? parentCategoryId)
        {
            await _blogCategoryService.AddBlogCategory(categoryName, parentCategoryId);
            return Ok("Add Blog Category successfully");
        }

        [HttpDelete("deleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteBlogCategory(int categoryId)
        {
            await _blogCategoryService.DeleteBlogCategory(categoryId);
            return Ok();
        }

        [HttpPut("updateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateBlogCategory(int categoryId, [FromBody] Dictionary<string, object> updates)
        {
            await _blogCategoryService.UpdateBlogCategory(categoryId, updates);
            return Ok();
        }

        [HttpGet("getBlogCategory")]
        public async Task<ActionResult<List<BlogCategory>>> GetAllBlogCategories()
        {
            var blogCategories = await _blogCategoryService.GetAllBlogCategories();
            return Ok(blogCategories);
        }

        [HttpGet("getBlogCategoryById/{categoryId}")]
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
