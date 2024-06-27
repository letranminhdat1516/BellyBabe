using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.BlogCategoryRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class BlogCategoryService
    {
        private readonly BlogCategoryRepository _blogCategoryRepository;

        public BlogCategoryService(BlogCategoryRepository blogCategoryRepository)
        {
            _blogCategoryRepository = blogCategoryRepository;
        }

        public async Task AddBlogCategory(string categoryName, int? parentCategoryId)
        {
            await _blogCategoryRepository.AddBlogCategory(categoryName, parentCategoryId);
        }

        public async Task DeleteBlogCategory(int categoryId)
        {
            await _blogCategoryRepository.DeleteBlogCategory(categoryId);
        }

        public async Task UpdateBlogCategory(int categoryId, string? categoryName, int? parentCategoryId)
        {
            await _blogCategoryRepository.UpdateBlogCategory(categoryId, categoryName, parentCategoryId);
        }

        public async Task<List<BlogCategory>> GetAllBlogCategories()
        {
            return await _blogCategoryRepository.GetAllBlogCategories();
        }

        public async Task<BlogCategory?> GetBlogCategoryById(int categoryId)
        {
            return await _blogCategoryRepository.GetBlogCategoryById(categoryId);
        }
    }
}
