using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.ProductRepository
{
    public class ProductRepository
    {
        private readonly Swp391Context _context;

        public ProductRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddProduct(string productName, bool isSelling, string madeIn, string unit, string suitableAge, string usageInstructions, string storageInstructions, int quantity, int categoryId, int brandId, int manufacturerId, int rating)
        {
            var newProduct = new Product
            {
                ProductName = productName,
                IsSelling = isSelling,
                MadeIn = madeIn,
                Unit = unit,
                SuitableAge = suitableAge,
                UsageInstructions = usageInstructions,
                StorageInstructions = storageInstructions,
                Quantity = quantity,
                CategoryId = categoryId,
                BrandId = brandId,
                ManufacturerId = manufacturerId,
                Rating = rating
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProduct(int productId, Dictionary<string, object> updates)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "productName":
                            product.ProductName = (string)update.Value;
                            break;
                        case "isSelling":
                            product.IsSelling = (bool)update.Value;
                            break;
                        case "madeIn":
                            product.MadeIn = (string)update.Value;
                            break;
                        case "unit":
                            product.Unit = (string)update.Value;
                            break;
                        case "suitableAge":
                            product.SuitableAge = (string)update.Value;
                            break;
                        case "usageInstructions":
                            product.UsageInstructions = (string)update.Value;
                            break;
                        case "storageInstructions":
                            product.StorageInstructions = (string)update.Value;
                            break;
                        case "quantity":
                            product.Quantity = (int)update.Value;
                            break;
                        case "categoryId":
                            product.CategoryId = (int)update.Value;
                            break;
                        case "brandId":
                            product.BrandId = (int)update.Value;
                            break;
                        case "manufacturerId":
                            product.ManufacturerId = (int)update.Value;
                            break;
                        case "rating":
                            product.Rating = (int)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        private T GetValue<T>(object value)
        {
            if (value is JsonElement jsonElement)
            {
                return jsonElement.ValueKind switch
                {
                    JsonValueKind.String => (T)(object)jsonElement.GetString(),
                    JsonValueKind.Number => jsonElement.TryGetInt32(out int intValue) ? (T)(object)intValue : (T)(object)jsonElement.GetDouble(),
                    JsonValueKind.True => (T)(object)true,
                    JsonValueKind.False => (T)(object)false,
                    _ => throw new InvalidOperationException($"Unsupported JsonValueKind: {jsonElement.ValueKind}")
                };
            }
            return (T)value;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var productsWithPrices = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.PriceUpdates.OrderByDescending(pr => pr.DateApplied).Take(2))
                .AsNoTracking()
                .ToListAsync();

            foreach (var product in productsWithPrices)
            {
                var recentPrices = product.PriceUpdates.OrderByDescending(pr => pr.DateApplied).Take(2).ToList();

                product.PriceUpdates = recentPrices;
            }

            return productsWithPrices;
        }

        public async Task<List<Product>> SearchProductByName(string name)
        {
            return await _context.Products.Where(p => p.ProductName.Contains(name))
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.PriceUpdates)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProductByStatus(bool isSelling = true)
        {
            return await _context.Products.Where(p => p.IsSelling == isSelling)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.PriceUpdates)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Product>> SortProductByPrice(bool ascending = true)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.PriceUpdates);

            if (ascending)
            {
                query = query.OrderBy(p => p.PriceUpdates.OrderByDescending(pr => pr.DateApplied).FirstOrDefault().Price);
            }
            else
            {
                query = query.OrderByDescending(p => p.PriceUpdates.OrderByDescending(pr => pr.DateApplied).FirstOrDefault().Price);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<List<Product>> SortProductByName(bool ascending = true)
        {
            return ascending ? await _context.Products.OrderBy(p => p.ProductName).Include(p => p.Brand).Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.PriceUpdates).AsNoTracking().ToListAsync()
                             : await _context.Products.OrderByDescending(p => p.ProductName).Include(p => p.Brand).Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.PriceUpdates).AsNoTracking().ToListAsync();
        }

        public async Task UpdateProductQuantity(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Quantity -= quantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}
