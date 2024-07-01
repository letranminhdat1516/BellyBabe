using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.RatingRepository
{
    public class RatingRepository
    {
        private readonly Swp391Context _context;

        public RatingRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddRating(int userId, int productId, int ratingValue)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                throw new ArgumentException("Người dùng không tồn tại.");
            }

            if (productId <= 0)
            {
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            }

            // Get the delivered status from the database
            var deliveredStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã giao hàng");
            if (deliveredStatus == null)
            {
                throw new ArgumentException("Status 'Đã giao hàng' not found.");
            }

            // Check if the user has bought and received the product
            var hasBoughtAndDelivered = await _context.Orders
                .Include(o => o.OrderDetails)
                .AnyAsync(o => o.UserId == userId &&
                               o.OrderStatuses.Any(os => os.StatusId == deliveredStatus.StatusId) &&
                               o.OrderDetails.Any(od => od.ProductId == productId));

            if (!hasBoughtAndDelivered)
            {
                throw new ArgumentException("Người dùng chưa mua hoặc nhận sản phẩm này.");
            }

            // Check if the user has already rated the product
            var alreadyRated = await _context.Ratings
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);

            if (alreadyRated)
            {
                throw new ArgumentException("Người dùng đã đánh giá sản phẩm này.");
            }

            var newRating = new Rating
            {
                UserId = userId,
                ProductId = productId,
                RatingValue = ratingValue,
                // RatingDate = ratingDate
            };

            _context.Ratings.Add(newRating);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRating(int ratingId)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("Đánh giá không tồn tại.");
            }
        }

        public async Task<bool> UpdateRating(int ratingId, int ratingValue)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);

            if (rating != null)
            {
                rating.RatingValue = ratingValue;
                // rating.RatingDate = ratingDate

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("Đánh giá không tồn tại.");
            }
        }

        public async Task<List<Rating>> GetAllRatings()
        {
            return await _context.Ratings
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Rating> GetRatingById(int ratingId)
        {
            return await _context.Ratings
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RatingId == ratingId);
        }

        public async Task<List<Rating>> GetRatingsByProductId(int productId)
        {
            return await _context.Ratings
                .Where(r => r.ProductId == productId)
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Rating>> GetRatingsByUserId(int userId)
        {
            return await _context.Ratings
                .Where(r => r.UserId == userId)
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
