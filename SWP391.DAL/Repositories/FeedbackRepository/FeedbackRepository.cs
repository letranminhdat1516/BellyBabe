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

        public async Task<Feedback> AddFeedbackAsync(int userId, string content, int rating)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Nội dung phản hồi không được để trống.");
            }

            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Đánh giá phải từ 1 đến 5.");
            }

            var hasDeliveredOrder = await _context.Orders
                .Include(o => o.Status)
                .AnyAsync(o => o.UserId == userId && o.Status.StatusName == "Đã giao hàng");

            if (!hasDeliveredOrder)
            {
                throw new InvalidOperationException("Người dùng chỉ có thể phản hồi sau khi đơn hàng đã được giao.");
            }

            var feedback = new Feedback
            {
                UserId = userId,
                Content = content,
                Rating = rating,
                DateCreated = DateTime.UtcNow
            };

            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.FeedbackResponses)
                .Where(f => f.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.FeedbackResponses)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.FeedbackResponses)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                return false;
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFeedbackAsync(int feedbackId, string content, int rating)
        {
            var existingFeedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (existingFeedback == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Nội dung phản hồi không được để trống.");
            }

            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Đánh giá phải từ 1 đến 5.");
            }

            existingFeedback.Content = content;
            existingFeedback.Rating = rating;

            _context.Feedbacks.Update(existingFeedback);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
