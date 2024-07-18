using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Feedback;
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

        public async Task<bool> HasUserProvidedFeedbackAsync(int userId, int orderId, int productId)
        {
            return await _context.Feedbacks
                .AnyAsync(f => f.UserId == userId &&
                               f.OrderDetail.OrderId == orderId &&
                               f.ProductId == productId);
        }

        public async Task<Feedback> CreateFeedbackAsync(int userId, int orderId, int productId, string content, int rating)
        {
            var hasProvidedFeedback = await HasUserProvidedFeedbackAsync(userId, orderId, productId);
            if (hasProvidedFeedback)
            {
                throw new InvalidOperationException("User has already provided feedback for this order and product.");
            }

            var order = await _context.Orders
                .Include(o => o.OrderStatuses)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found or does not belong to the user.");
            }

            if (!order.OrderStatuses.Any(os => os.StatusName == "Đã giao hàng"))
            {
                throw new InvalidOperationException("The order has not been delivered yet.");
            }

            var orderDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == productId);
            if (orderDetail == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found in the order.");
            }

            var ratingCategories = await _context.RatingCategories
                .Where(rc => rc.ProductId == productId)
                .ToListAsync();

            if (!ratingCategories.Any())
            {
                for (int i = 1; i <= 5; i++)
                {
                    ratingCategories.Add(new RatingCategory
                    {
                        CategoryName = $"{i} Star",
                        ProductId = productId,
                        TotalRatings = 0
                    });
                }
                _context.RatingCategories.AddRange(ratingCategories);
                await _context.SaveChangesAsync();
            }

            var appropriateCategory = ratingCategories.FirstOrDefault(rc => rc.CategoryName == $"{rating} Star");
            if (appropriateCategory == null)
            {
                throw new InvalidOperationException("Invalid rating value.");
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ProductId = productId,
                OrderDetailId = orderDetail.OrderDetailId,
                Content = content,
                Rating = rating,
                DateCreated = DateTime.Now,
                RatingCategoryId = appropriateCategory.CategoryId
            };

            _context.Feedbacks.Add(feedback);
            appropriateCategory.TotalRatings = (appropriateCategory.TotalRatings ?? 0) + 1;
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

        public async Task<Feedback> UpdateFeedbackAsync(int feedbackId, string content, int newRating)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                throw new InvalidOperationException("Feedback not found.");
            }

            int oldRating = feedback.Rating ?? 0;
            feedback.Content = content;
            feedback.Rating = newRating;

            if (oldRating != newRating)
            {
                var oldCategory = await _context.RatingCategories.FindAsync(feedback.RatingCategoryId);
                var newCategory = await _context.RatingCategories
                    .FirstOrDefaultAsync(rc => rc.ProductId == feedback.ProductId && rc.CategoryName == $"{newRating} Star");

                if (oldCategory != null)
                    oldCategory.TotalRatings = (oldCategory.TotalRatings ?? 1) - 1;

                if (newCategory != null)
                {
                    newCategory.TotalRatings = (newCategory.TotalRatings ?? 0) + 1;
                    feedback.RatingCategoryId = newCategory.CategoryId;
                }
            }

            await UpdateProductFeedbackStatsAsync(feedback.ProductId.Value, newRating, isNewFeedback: false, oldRating: oldRating);
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

            var category = await _context.RatingCategories.FindAsync(feedback.RatingCategoryId);
            if (category != null)
            {
                category.TotalRatings = (category.TotalRatings ?? 1) - 1;
            }

            _context.Feedbacks.Remove(feedback);
            await UpdateProductFeedbackStatsAsync(feedback.ProductId.Value, 0, isNewFeedback: false, oldRating: feedback.Rating ?? 0);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateProductFeedbackStatsAsync(int productId, int newRating, bool isNewFeedback, int oldRating = 0)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                if (isNewFeedback)
                    product.FeedbackTotal = (product.FeedbackTotal ?? 0) + 1;
                else if (newRating == 0)
                    product.FeedbackTotal = Math.Max((product.FeedbackTotal ?? 1) - 1, 0);

                var ratingCategories = await _context.RatingCategories
                    .Where(rc => rc.ProductId == productId)
                    .ToListAsync();

                int totalRatings = 0;
                int totalFeedbacks = 0;

                foreach (var category in ratingCategories)
                {
                    int categoryRating;
                    if (int.TryParse(category.CategoryName.Split(' ')[0], out categoryRating))
                    {
                        totalRatings += (category.TotalRatings ?? 0) * categoryRating;
                        totalFeedbacks += category.TotalRatings ?? 0;
                    }
                }

                product.NewPrice = totalFeedbacks > 0 ? (decimal)totalRatings / totalFeedbacks : 0;
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

        public async Task<List<FeedbackGroupByRating>> GetFeedbacksByProductIdAndRatingAsync(int productId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.ProductId == productId)
                .Include(f => f.User)
                .ToListAsync();

            var groupedFeedbacks = feedbacks
                .GroupBy(f => f.Rating ?? 0)
                .Select(g => new FeedbackGroupByRating
                {
                    Rating = g.Key,
                    Feedbacks = g.Select(f => new FeedbackInfo
                    {
                        Content = f.Content,
                        UserName = f.User.UserName,
                        DateCreated = f.DateCreated,
                        Rating = f.Rating ?? 0 
                    }).ToList()
                })
                .OrderByDescending(g => g.Rating)
                .ToList();

            return groupedFeedbacks;
        }
    }
}
