﻿using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.BLL.Services.ProductServices;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.BlogContent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost("AddBlock")]
        public async Task<IActionResult> AddBlog(int? userId, [FromBody] BlogContentModel blogContentModel, int? categoryId, string? titleName, string? image)
        {
            if (blogContentModel == null || string.IsNullOrWhiteSpace(blogContentModel.BlogContent))
            {
                return BadRequest("Invalid blog content.");
            }

            string sanitizedContent = blogContentModel.BlogContent.Replace("\n", "").Replace("\r", "");

            await _blogService.AddBlog(userId, blogContentModel.BlogContent, categoryId, titleName, image);
            return Ok("Blog đã được thêm thành công");
        }

        [HttpDelete("DeleteBlog/{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            await _blogService.DeleteBlog(blogId);
            return Ok("Blog đã được xóa thành công");
        }

        [HttpPut("UpdateBlog/{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId, int? userId, string? blogContent, int? categoryId, string? titleName, string? image)
        {
            await _blogService.UpdateBlog(blogId, userId, blogContent, categoryId, titleName, image);
            return Ok("Nội dung blog đã được cập nhật");
        }

        [HttpGet("GetAllBlogs")]
        public async Task<ActionResult<List<Blog>>> GetAllBlogs()
        {
            var blogs = await _blogService.GetAllBlogs();
            return Ok(blogs);
        }

        [HttpGet("GetBlogById/{blogId}")]
        public async Task<ActionResult<Blog?>> GetBlogById(int blogId)
        {
            var blog = await _blogService.GetBlogById(blogId);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpGet("GetBlogsByCategory/{categoryId}")]
        public async Task<ActionResult<List<Blog>>> GetBlogsByCategoryId(int categoryId)
        {
            var blogs = await _blogService.GetBlogsByCategoryId(categoryId);
            return Ok(blogs);
        }

        [HttpGet("GetBlogsByUserId/{userId}")]
        public async Task<ActionResult<List<Blog>>> GetBlogsByUserId(int userId)
        {
            var blogs = await _blogService.GetBlogsByUserId(userId);
            return Ok(blogs);
        }
    }
}
