using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
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

        public async Task UpdateProductCategory(int categoryId, Dictionary<string, object> updates)
        {
            var category = await _context.ProductCategories.FindAsync(categoryId);

            if (category != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "categoryName":
                            category.CategoryName = (string)update.Value;
                            break;
                        case "parentCategoryId":
                            category.ParentCategoryId = (int?)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
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
