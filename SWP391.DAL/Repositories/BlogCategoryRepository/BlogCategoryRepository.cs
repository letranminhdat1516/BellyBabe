using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.BlogCategoryRepository
{
    public class BlogCategoryRepository
    {
        private readonly Swp391Context _context;

        public BlogCategoryRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddBlogCategory(string categoryName, int? parentCategoryId)
        {
            var newBlogCategory = new BlogCategory
            {
                CategoryName = categoryName,
                ParentCategoryId = parentCategoryId
            };

            _context.BlogCategories.Add(newBlogCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlogCategory(int categoryId)
        {
            var blogCategory = await _context.BlogCategories.FindAsync(categoryId);
            if (blogCategory != null)
            {
                _context.BlogCategories.Remove(blogCategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBlogCategory(int categoryId, string? categoryName, int? parentCategoryId)
        {
            var blogCategory = await _context.BlogCategories.FindAsync(categoryId);

            if (blogCategory == null)
            {
                throw new ArgumentException("Danh mục blog không tồn tại.");
            }

            if (categoryName != null)
            {
                if (string.IsNullOrWhiteSpace(categoryName) || categoryName.Length > 100)
                {
                    throw new ArgumentException("Tên danh mục không được để trống và phải dưới 100 ký tự.");
                }

            }

            blogCategory.CategoryName = categoryName ?? blogCategory.CategoryName;
            blogCategory.ParentCategoryId = parentCategoryId ?? blogCategory.ParentCategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task<List<BlogCategory>> GetAllBlogCategories()
        {
            return await _context.BlogCategories
                .Include(c => c.Blogs)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BlogCategory?> GetBlogCategoryById(int categoryId)
        {
            return await _context.BlogCategories
                .Include(c => c.Blogs)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
    }
}
