using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.ProductCategoryRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class ProductCategoryService
    {
        private readonly ProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(ProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task AddProductCategory(string categoryName, int? parentCategoryId)
        {
            await _productCategoryRepository.AddProductCategory(categoryName, parentCategoryId);
        }

        public async Task DeleteProductCategory(int categoryId)
        {
            await _productCategoryRepository.DeleteProductCategory(categoryId);
        }

        public async Task UpdateProductCategory(int categoryId, string? categoryName, int? parentCategoryId)
        {
            await _productCategoryRepository.UpdateProductCategory(categoryId, categoryName, parentCategoryId);
        }

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _productCategoryRepository.GetAllProductCategories();
        }

        public async Task<ProductCategory?> GetProductCategoryById(int categoryId)
        {
            return await _productCategoryRepository.GetProductCategoryById(categoryId);
        }
    }
}
