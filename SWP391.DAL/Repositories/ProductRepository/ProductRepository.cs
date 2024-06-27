using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.ProductRepository
{
    public class ProductRepository
    {
        private readonly Swp391Context _context;
        private const int MaxSearchLength = 100;
        private const int MinSearchLength = 3;
        private static readonly Regex AllowedCharactersRegex = new Regex("^[a-zA-Z0-9 áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ ()-]*$");

        public ProductRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddProduct(string productName, bool? isSelling, string? description, int quantity, int isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int? oldPrice, decimal? discount, string? imageLinks)
        {
            if (string.IsNullOrWhiteSpace(productName) || productName.Length > 100)
            {
                throw new ArgumentException("Tên sản phẩm không được để trống và phải dưới 100 ký tự.");
            }

            if (!AllowedCharactersRegex.IsMatch(productName))
            {
                throw new ArgumentException("Tên sản phẩm chứa ký tự không hợp lệ.");
            }

            if (quantity < 0)
            {
                throw new ArgumentException("Số lượng sản phẩm không được nhỏ hơn 0.");
            }

            if (oldPrice.HasValue && oldPrice <= 0)
            {
                throw new ArgumentException("Giá sản phẩm không hợp lệ. Giá phải lớn hơn 0.");
            }

            if (discount.HasValue && (discount < 0 || discount > 100))
            {
                throw new ArgumentException("Giảm giá không hợp lệ. Giảm giá phải từ 0 đến 100%.");
            }

            var newProduct = new Product
            {
                ProductName = productName,
                IsSelling = isSelling,
                Description = description,
                Quantity = quantity,
                IsSoldOut = isSoldOut,
                BackInStockDate = backInStockDate,
                CategoryId = categoryId,
                BrandId = brandId,
                FeedbackTotal = feedbackTotal,
                OldPrice = oldPrice,
                Discount = discount,
                NewPrice = oldPrice.HasValue && discount.HasValue ? oldPrice.Value - (oldPrice.Value * (decimal)(discount / 100)) : (decimal?)null,
                ImageLinks = imageLinks
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                throw new ArgumentException("Sản phẩm không tồn tại.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(int productId, string? productName, bool? isSelling, string? description, int? quantity, int? isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, int? oldPrice, decimal? discount, string? imageLinks)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                throw new ArgumentException("Sản phẩm không tồn tại.");
            }

            if (productName != null)
            {
                if (string.IsNullOrWhiteSpace(productName) || productName.Length > 100)
                {
                    throw new ArgumentException("Tên sản phẩm không được để trống và phải dưới 100 ký tự.");
                }

                if (!AllowedCharactersRegex.IsMatch(productName))
                {
                    throw new ArgumentException("Tên sản phẩm chứa ký tự không hợp lệ.");
                }

                product.ProductName = productName;
            }

            if (quantity.HasValue && quantity < 0)
            {
                throw new ArgumentException("Số lượng sản phẩm không được nhỏ hơn 0.");
            }

            if (oldPrice.HasValue && oldPrice <= 0)
            {
                throw new ArgumentException("Giá sản phẩm không hợp lệ. Giá phải lớn hơn 0.");
            }

            if (discount.HasValue && (discount < 0 || discount > 100))
            {
                throw new ArgumentException("Giảm giá không hợp lệ. Giảm giá phải từ 0 đến 100%.");
            }

            product.IsSelling = isSelling ?? product.IsSelling;
            product.Description = description ?? product.Description;
            product.Quantity = quantity ?? product.Quantity;
            product.IsSoldOut = isSoldOut ?? product.IsSoldOut;
            product.BackInStockDate = backInStockDate ?? product.BackInStockDate;
            product.CategoryId = categoryId ?? product.CategoryId;
            product.BrandId = brandId ?? product.BrandId;
            product.FeedbackTotal = feedbackTotal ?? product.FeedbackTotal;
            product.OldPrice = oldPrice ?? product.OldPrice;
            product.Discount = discount ?? product.Discount;
            product.NewPrice = oldPrice.HasValue && discount.HasValue ? oldPrice.Value - (oldPrice.Value * (decimal)(discount / 100)) : product.NewPrice;
            product.ImageLinks = imageLinks ?? product.ImageLinks;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProductByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Vui lòng nhập một thuật ngữ tìm kiếm.", nameof(name));
            }

            name = name.Trim();

            if (name.Length > MaxSearchLength)
            {
                throw new ArgumentException($"Độ dài của truy vấn tìm kiếm không được vượt quá {MaxSearchLength} ký tự.", nameof(name));
            }

            if (name.Length < MinSearchLength)
            {
                throw new ArgumentException($"Độ dài của truy vấn tìm kiếm phải ít nhất {MinSearchLength} ký tự.", nameof(name));
            }

            var products = await _context.Products
                .Where(p => EF.Functions.Like(p.ProductName, $"%{name}%"))
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();

            if (products.Count == 0)
            {
                throw new ArgumentException("Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn.");
            }

            return products;
        }

        public async Task<List<Product>> SearchProductByStatus(bool isSelling = true)
        {
            var products = await _context.Products
                .Where(p => p.IsSelling == isSelling)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();

            if (products.Count == 0)
            {
                throw new ArgumentException("Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn.");
            }

            return products;
        }

        public async Task<List<Product>> SortProductByPrice(bool ascending = true)
        {
            var products = ascending
                ? await _context.Products.OrderBy(p => p.NewPrice)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync()
                : await _context.Products.OrderByDescending(p => p.NewPrice)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync();

            if (products.Count == 0)
            {
                throw new ArgumentException("Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn.");
            }

            return products;
        }

        public async Task<List<Product>> SortProductByName(bool ascending = true)
        {
            var products = ascending
                ? await _context.Products.OrderBy(p => p.ProductName)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync()
                : await _context.Products.OrderByDescending(p => p.ProductName)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync();

            if (products.Count == 0)
            {
                throw new ArgumentException("Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn.");
            }

            return products;
        }

        public async Task UpdateProductQuantity(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                if (product.Quantity < quantity)
                {
                    throw new ArgumentException("Số lượng sản phẩm không đủ để trừ.");
                }

                product.Quantity -= quantity;
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddFeedbackAsync(int productId, int userId, string content, int rating)
        {
            // Validate feedback content and rating

            var feedback = new Feedback
            {
                UserId = userId,
                Content = content,
                Rating = rating,
                DateCreated = DateTime.UtcNow
            };

            await _context.Feedbacks.AddAsync(feedback);

            // Update feedbackTotal for the product
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.FeedbackTotal++;
                _context.Products.Update(product);
            }

            await _context.SaveChangesAsync();
        }
    }
}
