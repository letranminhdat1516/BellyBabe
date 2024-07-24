using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.PreOrderRepository
{
    public class PreOrderRepository
    {
        private readonly Swp391Context _context;

        public PreOrderRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<PreOrder> AddPreOrderAsync(PreOrder preOrder)
        {
            await _context.PreOrders.AddAsync(preOrder);
            await _context.SaveChangesAsync();
            return preOrder;
        }
        public async Task<PreOrder> GetPreOrderByUserIdAndProductIdAsync(int userId, int productId)
        {
            return await _context.PreOrders
                .Include(p => p.User) // Bao gồm thông tin người dùng
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ProductId == productId);
        }
        public async Task<PreOrder> AddOrUpdatePreOrderAsync(PreOrder preOrder)
        {
            var existingPreOrder = await GetPreOrderByUserIdAndProductIdAsync(preOrder.UserId, preOrder.ProductId);

            if (existingPreOrder != null)
            {
                existingPreOrder.NotificationSent = preOrder.NotificationSent;

                _context.PreOrders.Update(existingPreOrder);
            }
            else
            {
                await _context.PreOrders.AddAsync(preOrder);
            }

            await _context.SaveChangesAsync();
            return preOrder;
        }
        public async Task<IEnumerable<PreOrder>> GetPreOrdersByUserIdAsync(int userId)
        {
            return await _context.PreOrders
                .Include(p => p.Product)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<PreOrder>> GetAllPreOrdersAsync()
        {
            return await _context.PreOrders
                .Include(p => p.Product)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task UpdateNotificationSentAsync(int preOrderId)
        {
            var preOrder = await _context.PreOrders.FindAsync(preOrderId);
            if (preOrder != null)
            {
                preOrder.NotificationSent = true;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> DeletePreOrderAsync(int preOrderId)
        {
            var preOrder = await _context.PreOrders.FindAsync(preOrderId);
            if (preOrder != null)
            {
                _context.PreOrders.Remove(preOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

 
    }
}
