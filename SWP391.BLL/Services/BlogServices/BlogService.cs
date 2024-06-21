using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.BlogRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task AddBlog(int? userId, string? blogContent, int? categoryId, string? titleName, DateTime? dateCreated)
        {
            await _blogRepository.AddBlog(userId, blogContent, categoryId, titleName, dateCreated);
        }

        public async Task DeleteBlog(int blogId)
        {
            await _blogRepository.DeleteBlog(blogId);
        }

        public async Task UpdateBlog(int blogId, Dictionary<string, object> updates)
        {
            await _blogRepository.UpdateBlog(blogId, updates);
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return await _blogRepository.GetAllBlogs();
        }

        public async Task<Blog?> GetBlogById(int blogId)
        {
            return await _blogRepository.GetBlogById(blogId);
        }

        public async Task<List<Blog>> GetBlogsByCategoryId(int categoryId)
        {
            return await _blogRepository.GetBlogsByCategoryId(categoryId);
        }

        public async Task<List<Blog>> GetBlogsByUserId(int userId)
        {
            return await _blogRepository.GetBlogsByUserId(userId);
        }
    }
}
