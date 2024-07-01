using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.CumulativeScoreRepository
{
    public class CumulativeScoreRepository
    {
        private readonly Swp391Context _context;

        public CumulativeScoreRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task UpdateCumulativeScoreAsync(int userId)
        {
            // Get the delivered status from the database
            var deliveredStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã giao hàng");
            if (deliveredStatus == null)
            {
                throw new ArgumentException("Status 'Đã giao hàng' not found.");
            }

            var completedOrders = await _context.Orders
                .Include(o => o.OrderStatuses)
                .Where(o => o.UserId == userId && o.OrderStatuses.Any(os => os.StatusId == deliveredStatus.StatusId))
                .ToListAsync();

            if (!completedOrders.Any()) return;

            var totalScore = completedOrders.Sum(o => ConvertTotalPriceToScore(o.TotalPrice ?? 0));

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.CumulativeScore = totalScore;
                await _context.SaveChangesAsync();
            }

            var cumulativeScoreEntry = new CumulativeScore
            {
                UserId = userId,
                TotalScore = totalScore,
                RatingCount = completedOrders.Count,
                DateCreated = DateTime.Now
            };

            _context.CumulativeScores.Add(cumulativeScoreEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetCumulativeScoreAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.CumulativeScore;
        }

        public async Task UpdateOrderScoreAsync(int orderId)
        {
            // Get the delivered status from the database
            var deliveredStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã giao hàng");
            if (deliveredStatus == null)
            {
                throw new ArgumentException("Status 'Đã giao hàng' not found.");
            }

            var order = await _context.Orders
                .Include(o => o.OrderStatuses)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.OrderStatuses.Any(os => os.StatusId == deliveredStatus.StatusId));

            if (order == null)
            {
                throw new InvalidOperationException("Order not found or not completed.");
            }

            var newScore = ConvertTotalPriceToScore(order.TotalPrice ?? 0);

            var user = await _context.Users.FindAsync(order.UserId);
            if (user != null)
            {
                user.CumulativeScore = (user.CumulativeScore ?? 0) + newScore;
                await _context.SaveChangesAsync();
            }
        }

        private int ConvertTotalPriceToScore(int totalPrice)
        {
            return totalPrice / 1000;
        }
    }
}
