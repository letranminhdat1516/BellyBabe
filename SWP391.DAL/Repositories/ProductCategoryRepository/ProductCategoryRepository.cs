using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.ProductCategoryRepository
{
    public class ProductCategoryRepository
    {
        private readonly Swp391Context _context;

        public ProductCategoryRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddProductCategory(string categoryName, int? parentCategoryId)
        {
            var newCategory = new ProductCategory
            {
                CategoryName = categoryName,
                ParentCategoryId = parentCategoryId
            };

            _context.ProductCategories.Add(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductCategory(int categoryId)
        {
            var category = await _context.ProductCategories.FindAsync(categoryId);
            if (category != null)
            {
                _context.ProductCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductCategory(int categoryId, string? categoryName, int? parentCategoryId)
        {
            var category = await _context.ProductCategories.FindAsync(categoryId);

            if (category == null)
            {
                throw new ArgumentException("Danh mục sản phẩm không tồn tại.");
            }

            if (categoryName != null)
            {
                if (string.IsNullOrWhiteSpace(categoryName) || categoryName.Length > 100)
                {
                    throw new ArgumentException("Tên danh mục không được để trống và phải dưới 100 ký tự.");
                }

            }

            category.CategoryName = categoryName ?? category.CategoryName;
            category.ParentCategoryId = parentCategoryId ?? category.ParentCategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _context.ProductCategories
                .Include(c => c.Products)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductCategory?> GetProductCategoryById(int categoryId)
        {
            return await _context.ProductCategories
                .Include(c => c.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
    }
}
