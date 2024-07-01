using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using SWP391.DAL.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.ProductRepository
{
    public class ProductRepository
    {
        private readonly Swp391Context _context;
        private const int MaxSearchLength = 100;
        private const int MinSearchLength = 3;
        private static readonly Regex AllowedCharactersRegex = new Regex("^[a-zA-Z0-9 áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ ,.()-]*$");

        public ProductRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddProduct(ProductModel productModel)
        {
            if (string.IsNullOrWhiteSpace(productModel.ProductName) || productModel.ProductName.Length > 100)
            {
                throw new ArgumentException("Tên sản phẩm không được để trống và phải dưới 100 ký tự.");
            }

            if (!AllowedCharactersRegex.IsMatch(productModel.ProductName))
            {
                throw new ArgumentException("Tên sản phẩm chứa ký tự không hợp lệ.");
            }

            if (productModel.Quantity < 0)
            {
                throw new ArgumentException("Số lượng sản phẩm không được nhỏ hơn 0.");
            }

            if (productModel.OldPrice.HasValue && productModel.OldPrice <= 0)
            {
                throw new ArgumentException("Giá sản phẩm không hợp lệ. Giá phải lớn hơn 0.");
            }

            if (productModel.Discount.HasValue && (productModel.Discount < 0 || productModel.Discount > 100))
            {
                throw new ArgumentException("Giảm giá không hợp lệ. Giảm giá phải từ 0 đến 100%.");
            }

            var newProduct = new Product
            {
                ProductName = productModel.ProductName,
                IsSelling = productModel.IsSelling ?? false,
                Description = productModel.Description,
                Quantity = productModel.Quantity,
                IsSoldOut = productModel.IsSoldOut,
                BackInStockDate = productModel.BackInStockDate,
                CategoryId = productModel.CategoryId,
                BrandId = productModel.BrandId,
                FeedbackTotal = productModel.FeedbackTotal ?? 0,
                OldPrice = productModel.OldPrice,
                Discount = productModel.Discount,
                NewPrice = productModel.OldPrice.HasValue && productModel.Discount.HasValue
                    ? productModel.OldPrice.Value - (productModel.OldPrice.Value * (decimal)(productModel.Discount.Value / 100))
                    : (decimal?)null,
                ImageLinks = productModel.ImageLinks
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

        public async Task UpdateProduct(ProductModel productModel)
        {
            var product = await _context.Products.FindAsync(productModel.ProductId);

            if (product == null)
            {
                throw new ArgumentException("Sản phẩm không tồn tại.");
            }

            product.ProductName = productModel.ProductName ?? product.ProductName;
            product.Quantity = productModel.Quantity;

            if (productModel.Quantity < 0)
            {
                throw new ArgumentException("Số lượng sản phẩm không được nhỏ hơn 0.");
            }

            if (productModel.OldPrice.HasValue)
            {
                if (productModel.OldPrice <= 0)
                {
                    throw new ArgumentException("Giá sản phẩm không hợp lệ. Giá phải lớn hơn 0.");
                }
                product.OldPrice = productModel.OldPrice.Value;
            }

            if (productModel.Discount.HasValue)
            {
                if (productModel.Discount < 0 || productModel.Discount > 100)
                {
                    throw new ArgumentException("Giảm giá không hợp lệ. Giảm giá phải từ 0 đến 100%.");
                }
                product.Discount = productModel.Discount.Value;
                product.NewPrice = productModel.OldPrice.HasValue
                    ? productModel.OldPrice.Value - (productModel.OldPrice.Value * (decimal)(productModel.Discount.Value / 100))
                    : product.NewPrice;
            }

            product.IsSelling = productModel.IsSelling ?? product.IsSelling;
            product.Description = productModel.Description ?? product.Description;
            product.IsSoldOut = productModel.IsSoldOut;
            product.BackInStockDate = productModel.BackInStockDate;
            product.CategoryId = productModel.CategoryId ?? product.CategoryId;
            product.BrandId = productModel.BrandId ?? product.BrandId;
            product.FeedbackTotal = productModel.FeedbackTotal ?? product.FeedbackTotal;
            product.ImageLinks = productModel.ImageLinks ?? product.ImageLinks;

            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            var products = await _context.Products
                                .AsNoTracking()
                                .Select(p => new ProductModel
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    IsSelling = p.IsSelling,
                                    Description = p.Description,
                                    Quantity = p.Quantity,
                                    IsSoldOut = p.IsSoldOut,
                                    BackInStockDate = p.BackInStockDate,
                                    CategoryId = p.CategoryId,
                                    BrandId = p.BrandId,
                                    FeedbackTotal = p.FeedbackTotal,
                                    OldPrice = p.OldPrice,
                                    Discount = p.Discount,
                                    NewPrice = p.NewPrice,
                                    ImageLinks = p.ImageLinks
                                })
                                .ToListAsync();

            return products;
        }

        public async Task<List<ProductModel>> SearchProductByName(string name)
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
                .AsNoTracking()
                .Select(p => new ProductModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    IsSelling = p.IsSelling,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    IsSoldOut = p.IsSoldOut,
                    BackInStockDate = p.BackInStockDate,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    FeedbackTotal = p.FeedbackTotal,
                    OldPrice = p.OldPrice,
                    Discount = p.Discount,
                    NewPrice = p.NewPrice,
                    ImageLinks = p.ImageLinks
                })
                .ToListAsync();

            if (products.Count == 0)
            {
                throw new ArgumentException("Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn.");
            }

            return products;
        }

        public async Task<List<ProductModel>> SearchProductByStatus(bool isSelling = true)
        {
            var products = await _context.Products
                .Where(p => p.IsSelling == isSelling)
                .AsNoTracking()
                .Select(p => new ProductModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    IsSelling = p.IsSelling,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    IsSoldOut = p.IsSoldOut,
                    BackInStockDate = p.BackInStockDate,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    FeedbackTotal = p.FeedbackTotal,
                    OldPrice = p.OldPrice,
                    Discount = p.Discount,
                    NewPrice = p.NewPrice,
                    ImageLinks = p.ImageLinks
                })
                .ToListAsync();

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
    }
}