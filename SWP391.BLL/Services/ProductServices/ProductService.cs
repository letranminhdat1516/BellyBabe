using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.ProductRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.ProductServices
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProduct(string productName, bool isSelling,  string madeIn, string unit, string suitableAge, string usageInstructions, string storageInstructions, int quantity, int categoryId, int brandId, int manufacturerId, int rating)
        {
            await _productRepository.AddProduct(productName, isSelling, madeIn, unit, suitableAge, usageInstructions, storageInstructions, quantity, categoryId, brandId, manufacturerId, rating);
        }

        public async Task DeleteProduct(int productId)
        {
            await _productRepository.DeleteProduct(productId);
        }

        public async Task<List<Product>> SearchProductByName(string name)
        {
            return await _productRepository.SearchProductByName(name);
        }

        public async Task<List<Product>> SearchProductByStatus(bool isSelling)
        {
            return await _productRepository.SearchProductByStatus(isSelling);
        }

        public async Task<List<Product>> ShowAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<List<Product>> SortProductByName(bool ascending = true)
        {
            return await _productRepository.SortProductByName(ascending);
        }

        public async Task<List<Product>> SortProductByPrice(bool ascending = true)
        {
            return await _productRepository.SortProductByPrice(ascending);
        }

        public async Task UpdateProduct(int productId, Dictionary<string, object> updates)
        {
            await _productRepository.UpdateProduct(productId, updates);
        }
    }
}
