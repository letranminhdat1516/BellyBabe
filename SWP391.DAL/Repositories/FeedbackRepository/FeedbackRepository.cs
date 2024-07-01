using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.FeedbackRepository
{
    public class FeedbackRepository
    {
        private readonly Swp391Context _context;

        public FeedbackRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<Feedback> CreateFeedbackAsync(int userId, int productId, string content, int rating)
        {
            // Check if the user has purchased the product and if the order status is "Đã giao hàng"
            var order = await _context.Orders
                .Include(o => o.OrderStatuses)
                .Where(o => o.UserId == userId &&
                            o.OrderDetails.Any(od => od.ProductId == productId) &&
                            o.OrderStatuses.Any(os => os.StatusName == "Đã giao hàng"))
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new InvalidOperationException("User has not purchased this product or the order has not been delivered yet.");
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ProductId = productId,
                Content = content,
                Rating = rating,
                DateCreated = DateTime.Now,
            };

            _context.Feedbacks.Add(feedback);

            // Update FeedbackTotal for the product
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.FeedbackTotal = (product.FeedbackTotal ?? 0) + 1;
            }

            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _context.Feedbacks
               // .Include(f => f.User)
               // .Include(f => f.Product)
               // .Include(f => f.RatingCategory)
                .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
        }

        public async Task<List<Feedback>> GetFeedbacksByProductIdAsync(int productId)
        {
            return await _context.Feedbacks
                .Where(f => f.ProductId == productId)
               // .Include(f => f.User)
               // .Include(f => f.RatingCategory)
                .ToListAsync();
        }

        public async Task<Feedback> UpdateFeedbackAsync(int feedbackId, string content, int rating)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                throw new InvalidOperationException("Feedback not found.");
            }

            feedback.Content = content;
            feedback.Rating = rating;

            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                throw new InvalidOperationException("Feedback not found.");
            }

            _context.Feedbacks.Remove(feedback);

            // Decrease FeedbackTotal for the product
            var product = await _context.Products.FindAsync(feedback.ProductId);
            if (product != null)
            {
                product.FeedbackTotal = Math.Max((product.FeedbackTotal ?? 0) - 1, 0);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            var ratings = await _context.Feedbacks
                .Where(f => f.ProductId == productId)
                .Select(f => f.Rating ?? 0)
                .ToListAsync();

            if (ratings.Any())
            {
                return ratings.Average();
            }

            return 0;
        }

        public async Task<List<Feedback>> GetRecentFeedbacksAsync(int count)
        {
            return await _context.Feedbacks
                .OrderByDescending(f => f.DateCreated)
                .Take(count)
                .Include(f => f.User)
                .Include(f => f.Product)
                .Include(f => f.RatingCategory)
                .ToListAsync();
        }

        public async Task<bool> HasUserPurchasedProductAsync(int userId, int productId)
        {
            return await _context.Orders
                .AnyAsync(o => o.UserId == userId &&
                               o.OrderDetails.Any(od => od.ProductId == productId) &&
                               o.OrderStatuses.Any(os => os.StatusName == "Đã giao hàng"));
        }

        public async Task<List<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _context.Feedbacks
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .Include(f => f.RatingCategory)
                .ToListAsync();
        }
    }
}