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

        public async Task AddProduct(string productName, bool? isSelling, string? description, int quantity, int isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int oldPrice, decimal discount, string imageLinks)
        {
            // Convert necessary parameters to strings
            string quantityStr = quantity.ToString();
            string oldPriceStr = oldPrice.ToString();
            string discountStr = discount.ToString("F1"); // One decimal place

            await _productRepository.AddProduct(productName, isSelling, description, quantityStr, isSoldOut, backInStockDate, categoryId, brandId, feedbackTotal, oldPriceStr, discountStr, imageLinks);
        }

        public async Task DeleteProduct(int productId)
        {
            await _productRepository.DeleteProduct(productId);
        }

        public async Task<List<Product>> SearchProductByName(string name)
        {
            return await _productRepository.SearchProductByName(name);
        }

        public async Task<List<Product>> SearchProductByStatus(bool isSelling = true)
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
            // Ensure numeric and date values are converted to the correct types
            var convertedUpdates = updates.ToDictionary(
                kvp => kvp.Key,
                kvp =>
                {
                    return kvp.Key switch
                    {
                        nameof(Product.Quantity) => int.Parse(kvp.Value.ToString()),
                        nameof(Product.IsSoldOut) => int.Parse(kvp.Value.ToString()),
                        nameof(Product.OldPrice) => int.Parse(kvp.Value.ToString()),
                        nameof(Product.Discount) => decimal.Parse(kvp.Value.ToString()),
                        nameof(Product.BackInStockDate) => DateTime.Parse(kvp.Value.ToString()),
                        _ => kvp.Value
                    };
                }
            );

            await _productRepository.UpdateProduct(productId, convertedUpdates);
        }

        public async Task UpdateProductQuantity(int productId, int quantity)
        {
            await _productRepository.UpdateProductQuantity(productId, quantity);
        }
    }
}
