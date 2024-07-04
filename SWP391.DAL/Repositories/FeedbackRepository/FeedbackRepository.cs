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

        public async Task<Feedback> CreateFeedbackAsync(int userId, int orderId, int productId, string content, int rating, int? ratingCategoryId)
        {
            // Check if the order exists and belongs to the user
            var order = await _context.Orders
                .Include(o => o.OrderStatuses)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found or does not belong to the user.");
            }

            // Check if the order status is "Đã giao hàng"
            if (!order.OrderStatuses.Any(os => os.StatusName == "Đã giao hàng"))
            {
                throw new InvalidOperationException("The order has not been delivered yet.");
            }

            // Find the order detail for the specified product
            var orderDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == productId);
            if (orderDetail == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found in the order.");
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ProductId = productId,
                OrderDetailId = orderDetail.OrderDetailId,
                Content = content,
                Rating = rating,
                DateCreated = DateTime.Now,
                RatingCategoryId = ratingCategoryId
            };

            _context.Feedbacks.Add(feedback);

            // Update FeedbackTotal and AverageRating for the product
            await UpdateProductFeedbackStatsAsync(productId, rating, isNewFeedback: true);

            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
        }

        public async Task<List<Feedback>> GetFeedbacksByProductIdAsync(int productId)
        {
            return await _context.Feedbacks
                .Where(f => f.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Feedback> UpdateFeedbackAsync(int feedbackId, string content, int newRating, int? ratingCategoryId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                throw new InvalidOperationException("Feedback not found.");
            }

            int oldRating = feedback.Rating ?? 0;
            feedback.Content = content;
            feedback.Rating = newRating;
            feedback.RatingCategoryId = ratingCategoryId;

            // Update AverageRating for the product
            await UpdateProductFeedbackStatsAsync(feedback.ProductId.Value, newRating, oldRating, isUpdate: true);

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

            // Update FeedbackTotal and AverageRating for the product
            await UpdateProductFeedbackStatsAsync(feedback.ProductId.Value, feedback.Rating ?? 0, isNewFeedback: false);

            await _context.SaveChangesAsync();
        }

        private async Task UpdateProductFeedbackStatsAsync(int productId, int rating, int? oldRating = null, bool isNewFeedback = true, bool isUpdate = false)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                if (isNewFeedback)
                {
                    product.FeedbackTotal = (product.FeedbackTotal ?? 0) + 1;
                }
                else if (!isUpdate)
                {
                    product.FeedbackTotal = Math.Max((product.FeedbackTotal ?? 0) - 1, 0);
                }

                var allRatings = await _context.Feedbacks
                    .Where(f => f.ProductId == productId)
                    .Select(f => f.Rating)
                    .ToListAsync();

                if (isNewFeedback)
                {
                    allRatings.Add(rating);
                }
                else if (isUpdate && oldRating.HasValue)
                {
                    allRatings.Remove(oldRating.Value);
                    allRatings.Add(rating);
                }
                else if (!isUpdate)
                {
                    allRatings.Remove(rating);
                }

                product.NewPrice = allRatings.Any() ? (decimal)allRatings.Average() : 0;

                _context.Products.Update(product);
            }
        }

        public async Task<decimal> GetAverageRatingForProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product?.NewPrice ?? 0;
        }

        public async Task<List<Feedback>> GetRecentFeedbacksAsync(int count)
        {
            return await _context.Feedbacks
                .OrderByDescending(f => f.DateCreated)
                .Take(count)
                .Include(f => f.User)
                .Include(f => f.Product)
                .ToListAsync();
        }

        public async Task<bool> CanUserProvideFeedbackAsync(int userId, int productId, int orderDetailId)
        {
            return await _context.OrderDetails
                .AnyAsync(od => od.OrderDetailId == orderDetailId &&
                                od.UserId == userId &&
                                od.ProductId == productId &&
                                od.Order.OrderStatuses.Any(os => os.StatusName == "Đã giao hàng") &&
                                !_context.Feedbacks.Any(f => f.OrderDetailId == od.OrderDetailId && f.ProductId == productId));
        }

        public async Task<List<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _context.Feedbacks
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .ToListAsync();
        }
    }
}