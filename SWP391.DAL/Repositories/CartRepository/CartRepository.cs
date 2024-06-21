using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;

namespace SWP391.DAL.Repositories.CartRepository
{
    public class CartRepository
    {
        private readonly Swp391Context _context;

        public CartRepository(Swp391Context context)
        {
            _context = context; 
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Invalid product ID.");
            }

            var priceUpdates = await _context.PriceUpdates
                .Where(p => p.ProductId == productId)
                .OrderByDescending(p => p.DateApplied)
                .Take(2)
                .ToListAsync();

            if (priceUpdates.Count == 0)
            {
                throw new Exception("No price updates found for the product.");
            }

            var latestPrice = priceUpdates.First().Price;
            var oldPrice = priceUpdates.Skip(1).FirstOrDefault()?.Price ?? 0;

            var orderDetail = new OrderDetail
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                Price = latestPrice
            };

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDetail>> GetCartDetailsAsync(int userId)
        {
            return await _context.OrderDetails
                .Where(od => od.UserId == userId && od.OrderId == null)
                .ToListAsync();
        }

        public async Task IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

            if (orderDetail != null)
            {
                orderDetail.Quantity += quantityToAdd;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

            if (orderDetail != null)
            {
                orderDetail.Quantity -= quantityToSubtract;
                if (orderDetail.Quantity <= 0)
                {
                    _context.OrderDetails.Remove(orderDetail);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductFromCartAsync(int userId, int productId)
        {
            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.UserId == userId && od.ProductId == productId && od.OrderId == null);

            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
