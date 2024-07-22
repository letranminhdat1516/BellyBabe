using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;

namespace SWP391.DAL.Repositories
{
    public class CartRepository
    {
        private readonly Swp391Context _context;

        public CartRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<OrderDetail> GetOrderDetailAsync(int userId, int productId)
        {
            return await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDetail>> GetCartDetailsAsync(int userId)
        {
            return await _context.OrderDetails
                .Where(od => od.UserId == userId && od.OrderId == null)
                .Include(od => od.Product)
                .ToListAsync();
        }

        public async Task<OrderDetail> AddToCartAsync(int userId, int productId, int quantity, bool isChecked)
        {
            // Fetch the product
            var product = await GetProductAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("ID sản phẩm không hợp lệ.");
            }

            // Check if the product is sold out
            if (product.IsSoldOut == 1)
            {
                throw new ArgumentException("Sản phẩm đã hết hàng và không thể thêm vào giỏ.");
            }

            // Get the existing order detail for the user and product
            var orderDetail = await GetOrderDetailAsync(userId, productId);
            if (orderDetail == null)
            {
                // Create a new order detail if it doesn't exist
                orderDetail = new OrderDetail
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = (int)(product.NewPrice * quantity),
                    IsChecked = isChecked
                };
                await AddOrderDetailAsync(orderDetail);
            }
            else
            {
                // Update the existing order detail
                orderDetail.Quantity += quantity;
                orderDetail.Price = (int)(product.NewPrice * orderDetail.Quantity);
                orderDetail.IsChecked = isChecked;
                await UpdateOrderDetailAsync(orderDetail);
            }

            return orderDetail;
        }

        public async Task<OrderDetail> PurchaseNowAsync(int userId, int productId, int quantity)
        {
            var product = await GetProductAsync(productId);

            if (product.IsSoldOut == 1)
            {
                throw new ArgumentException("Sản phẩm đã hết hàng và không thể thêm vào giỏ.");
            }

            return await AddToCartAsync(userId, productId, quantity, true);
        }

        public async Task UpdateIsCheckedAsync(int userId, int productId, bool isChecked)
        {
            var orderDetail = await GetOrderDetailAsync(userId, productId);
            if (orderDetail != null)
            {
                orderDetail.IsChecked = isChecked;
                await UpdateOrderDetailAsync(orderDetail);
            }
        }
    }
}