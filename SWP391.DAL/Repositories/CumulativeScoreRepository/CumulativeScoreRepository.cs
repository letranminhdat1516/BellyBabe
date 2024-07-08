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

        public async Task<int> GetUserCumulativeScoreAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            return user.CumulativeScore ?? 0;
        }

        public async Task AddPointsAsync(int userId, int points)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            user.CumulativeScore += points;
            await _context.SaveChangesAsync();
        }

        public async Task UsePointsAsync(int userId, int points)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            if (user.CumulativeScore < points)
            {
                throw new InvalidOperationException("Không đủ điểm để sử dụng.");
            }

            user.CumulativeScore -= points;
            await _context.SaveChangesAsync();
        }
    }
}
