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

        public async Task<bool> AddRating(int? userId, int? productId, int? ratingValue, DateTime? ratingDate)
        {
            if (userId == null || productId == null)
            {
                return false;
            }

            var hasBoughtAndDelivered = await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Status)
                .AnyAsync(o => o.UserId == userId &&
                               o.Status.StatusName == "Đã giao hàng" &&
                               o.OrderDetails.Any(od => od.ProductId == productId));

            if (!hasBoughtAndDelivered)
            {
                return false;
            }

            var newRating = new Rating
            {
                UserId = userId,
                ProductId = productId,
                RatingValue = ratingValue,
                RatingDate = ratingDate
            };

            _context.Ratings.Add(newRating);
            await _context.SaveChangesAsync();
            return true;
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
            return false;
        }

        public async Task<bool> UpdateRating(int ratingId, Dictionary<string, object> updates)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);

            if (rating != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "userId":
                            rating.UserId = (int?)update.Value;
                            break;
                        case "productId":
                            rating.ProductId = (int?)update.Value;
                            break;
                        case "ratingValue":
                            rating.RatingValue = (int?)update.Value;
                            break;
                        case "ratingDate":
                            rating.RatingDate = (DateTime?)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Rating>> GetAllRatings()
        {
            return await _context.Ratings
                .Include(r => r.Product)
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Rating?> GetRatingById(int ratingId)
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
