using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.ProductRepository;
using SWP391.DAL.Model.Product;
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

        public async Task AddProduct(string productName, string? description, int quantity, int? categoryId, int? brandId, int? oldPrice, decimal? discount, string? imageLinks)
        {
            var productModel = new ProductModel
            {
                ProductName = productName,
                Description = description,
                Quantity = quantity,
                CategoryId = categoryId,
                BrandId = brandId,
                OldPrice = oldPrice,
                Discount = discount,
                ImageLinks = imageLinks
            };

            await _productRepository.AddProduct(productModel);
        }

        public async Task DeleteProduct(int productId)
        {
            await _productRepository.DeleteProduct(productId);
        }

        public async Task<List<ProductModel>> SearchProductByName(string name)
        {
            return await _productRepository.SearchProductByName(name);
        }

        public async Task<List<ProductModel>> SearchProductByStatus(bool isSelling)
        {
            return await _productRepository.SearchProductByStatus(isSelling);
        }

        public async Task<List<ProductModel>> ShowAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<List<ProductModel>> SortProductByName(bool ascending = true)
        {
            var products = await _productRepository.GetAllProducts();
            return ascending
                ? products.OrderBy(p => p.ProductName).ToList()
                : products.OrderByDescending(p => p.ProductName).ToList();
        }

        public async Task<List<ProductModel>> SortProductByPrice(bool ascending = true)
        {
            var products = await _productRepository.GetAllProducts();
            return ascending
                ? products.OrderBy(p => p.OldPrice).ToList()
                : products.OrderByDescending(p => p.OldPrice).ToList();
        }

        public async Task<ProductModel> GetProductById(int productId)
        {
            var products = await _productRepository.GetAllProducts();
            var product = products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new ArgumentException("Sản phẩm không tồn tại.");
            }
            return product;
        }

        public async Task UpdateProduct(int productId, string? productName, string? description, int? quantity, int? categoryId, int? brandId, int? oldPrice, decimal? discount, string? imageLinks)
        {
            var productModel = new ProductModel
            {
                ProductId = productId,
                ProductName = productName,
                Description = description,
                Quantity = quantity ?? 0,
                CategoryId = categoryId,
                BrandId = brandId,
                OldPrice = oldPrice,
                Discount = discount,
                ImageLinks = imageLinks
            };

            await _productRepository.UpdateProduct(productModel);
        }

        public async Task UpdateProductQuantity(int productId, int quantity)
        {
            var product = await GetProductById(productId);
            product.Quantity = quantity;
            await _productRepository.UpdateProduct(product);
        }

        public async Task UpdateProductImageLinksAsync(int productId, List<string> imageLinks)
        {
            var imageLinksString = string.Join(",", imageLinks);
            await _productRepository.UpdateProductImageLinksAsync(productId, imageLinksString);
        }
    }
}