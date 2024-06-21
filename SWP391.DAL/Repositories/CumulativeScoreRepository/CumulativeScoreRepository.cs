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
            var completedOrders = await _context.Orders
                .Where(o => o.UserId == userId && o.Status.StatusName == "Đã giao hàng")
                .ToListAsync();

            if (!completedOrders.Any()) return;

            var totalScore = completedOrders.Sum(o => o.TotalPrice ?? 0);

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.CumulativeScore = (int?)totalScore;
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

        public async Task<decimal?> GetCumulativeScoreAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.CumulativeScore;
        }
    }
}
