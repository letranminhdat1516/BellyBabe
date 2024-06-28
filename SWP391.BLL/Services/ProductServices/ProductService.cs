using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.ProductRepository;
using System;
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

        public async Task AddProduct(string productName, bool? isSelling, string? description, int quantity, int isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int? oldPrice, decimal? discount, string? imageLinks)
        {
            await _productRepository.AddProduct(productName, isSelling, description, quantity, isSoldOut, backInStockDate, categoryId, brandId, feedbackTotal, oldPrice, discount, imageLinks);
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

        public async Task<Product> GetProductById(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }

        public async Task UpdateProduct(int productId, string? productName, bool? isSelling, string? description, int? quantity, int? isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int? oldPrice, decimal? discount, string? imageLinks)
        {
            await _productRepository.UpdateProduct(productId, productName, isSelling, description, quantity, isSoldOut, backInStockDate, categoryId, brandId, feedbackTotal, oldPrice, discount, imageLinks);
        }

        public async Task UpdateProductQuantity(int productId, int quantity)
        {
            await _productRepository.UpdateProductQuantity(productId, quantity);
        }
    }
}
