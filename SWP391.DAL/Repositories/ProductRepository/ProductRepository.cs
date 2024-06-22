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
        private static readonly Regex AllowedCharactersRegex = new Regex("^[a-zA-Z0-9 áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ ]*$");

        public ProductRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddProduct(string productName, bool? isSelling, string? description, string quantity, int isSoldOut, DateTime? backInStockDate, int? categoryId, int? brandId, int? feedbackTotal, string oldPrice, string discount, string imageLinks)
        {
            if (string.IsNullOrEmpty(productName) || !Regex.IsMatch(productName, @"^[a-zA-Z0-9áíýớúàèõưấịờựãẽõũẮỊỚỨŨÀÈÕƯẤĨỚỨŨ ]+$"))
            {
                throw new ArgumentException(nameof(productName), "Tên sản phẩm không được chứa ký tự đặc biệt và không được để trống.");
            }

            if (!int.TryParse(quantity, out int parsedQuantity) || parsedQuantity <= 0)
            {
                throw new ArgumentException(nameof(quantity), "Số lượng chỉ được chứa các chữ số, phải lớn hơn 0 và không có ký tự đặc biệt."); 
            }

            if (!int.TryParse(oldPrice, out int parsedOldPrice) || parsedOldPrice <= 0)
            {
                throw new ArgumentException(nameof(oldPrice), "Giá sản phẩm không hợp lệ. Giá phải lớn hơn 0.");
            }

            if (!decimal.TryParse(discount, out decimal parsedDiscount) || parsedDiscount < 0 || parsedDiscount > 100)
            {
                throw new ArgumentException(nameof(discount), "Giảm giá không hợp lệ. Giảm giá phải từ 0 đến 100%."); 
            }

            var newProduct = new Product
            {
                ProductName = productName,
                IsSelling = isSelling,
                Description = description,
                Quantity = parsedQuantity,
                IsSoldOut = isSoldOut,
                BackInStockDate = backInStockDate,
                CategoryId = categoryId,
                BrandId = brandId,
                FeedbackTotal = feedbackTotal,
                OldPrice = parsedOldPrice,
                Discount = parsedDiscount,
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
                throw new ArgumentException(nameof(productId), "Sản phẩm không tồn tại."); 
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(int productId, Dictionary<string, object> updates)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                throw new ArgumentException(nameof(productId), "Sản phẩm không tồn tại."); 
            }

            foreach (var update in updates)
            {
                switch (update.Key)
                {
                    case nameof(Product.ProductName):
                        if (!Regex.IsMatch((string)update.Value, @"^[a-zA-Z0-9áíýớúàèõưấịờựãẽõũẮỊỚỨŨÀÈÕƯẤĨỚỨŨ ]+$"))
                        {
                            throw new ArgumentException(nameof(update.Key), "Tên sản phẩm không được chứa ký tự đặc biệt (ngoại trừ dấu cách và dấu tiếng Việt).");
                        }
                        product.ProductName = (string)update.Value;
                        break;
                    case nameof(Product.IsSelling):
                        product.IsSelling = (bool?)update.Value;
                        break;
                    case nameof(Product.Quantity):
                        if (!(update.Value is int) || (int)update.Value <= 0)
                        {
                            throw new ArgumentException(nameof(update.Key), "Số lượng chỉ được chứa các chữ số và phải lớn hơn 0.");
                        }
                        product.Quantity = (int)update.Value;
                        break;
                    case nameof(Product.IsSoldOut):
                        if (!(update.Value is int) || ((int)update.Value != 0 && (int)update.Value != 1))
                        {
                            throw new ArgumentException(nameof(update.Key), "Giá trị 'IsSoldOut' chỉ được là 0 hoặc 1."); 
                        }
                        product.IsSoldOut = (int)update.Value;
                        break;
                    case nameof(Product.BackInStockDate):
                        product.BackInStockDate = (DateTime?)update.Value;
                        break;
                    case nameof(Product.CategoryId):
                        product.CategoryId = (int?)update.Value;
                        break;
                    case nameof(Product.BrandId):
                        product.BrandId = (int?)update.Value;
                        break;
                    case nameof(Product.FeedbackTotal):
                        product.FeedbackTotal = (int?)update.Value;
                        break;
                    case nameof(Product.OldPrice):
                        if (!(update.Value is int) || (int)update.Value <= 0)
                        {
                            throw new ArgumentException(nameof(update.Key), "Giá sản phẩm không hợp lệ. Giá phải lớn hơn 0."); 
                        }
                        product.OldPrice = (int)update.Value;
                        break;
                    case nameof(Product.Discount):
                        if (!(update.Value is decimal) || (decimal)update.Value < 0 || (decimal)update.Value > 100 || Math.Round((decimal)update.Value, 1) != (decimal)update.Value)
                        {
                            throw new ArgumentException(nameof(update.Key), "Giảm giá không hợp lệ. Giảm giá phải từ 0 đến 100% và chỉ được có 1 chữ số sau dấu phẩy."); // Invalid discount. Discount must be between 0 and 100%, and can only have one decimal place.
                        }
                        product.Discount = (decimal?)update.Value;
                        break;
                    case nameof(Product.ImageLinks):
                        product.ImageLinks = (string)update.Value;
                        break;
                    default:
                        throw new ArgumentException($"Tên thuộc tính không hợp lệ: {update.Key}", nameof(updates));
                }
            }
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

            name = NormalizeString(name.Trim().ToLower());

            if (name.Length > MaxSearchLength)
            {
                throw new ArgumentException($"Độ dài của truy vấn tìm kiếm không được vượt quá {MaxSearchLength} ký tự.", nameof(name));
            }

            if (name.Length < MinSearchLength)
            {
                throw new ArgumentException($"Độ dài của truy vấn tìm kiếm phải ít nhất {MinSearchLength} ký tự.", nameof(name));
            }

            if (!AllowedCharactersRegex.IsMatch(name))
            {
                throw new ArgumentException("Truy vấn tìm kiếm chứa các ký tự không hợp lệ.", nameof(name));
            }

            var products = await _context.Products
                .Where(p => NormalizeString(p.ProductName.ToLower()).Contains(name))
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
                product.Quantity -= quantity;
                await _context.SaveChangesAsync();
            }
        }

        private string NormalizeString(string input)
        {
            string formD = input.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in formD)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
