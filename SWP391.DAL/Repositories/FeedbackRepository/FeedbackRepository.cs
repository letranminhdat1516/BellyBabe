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

        public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback), "Phản hồi không được để trống.");
            }

            if (feedback.UserId == null || feedback.Rating == null || string.IsNullOrEmpty(feedback.Content))
            {
                throw new ArgumentException("Phản hồi phải chứa ID người dùng, nội dung và đánh giá.");
            }

            feedback.DateCreated = DateTime.UtcNow;

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

        public async Task<Feedback?> GetFeedbackByIdAsync(int feedbackId)
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

        public async Task<bool> UpdateFeedbackAsync(Feedback feedback)
        {
            var existingFeedback = await _context.Feedbacks.FindAsync(feedback.FeedbackId);
            if (existingFeedback == null)
            {
                return false;
            }

            existingFeedback.Content = feedback.Content ?? existingFeedback.Content;
            existingFeedback.Rating = feedback.Rating ?? existingFeedback.Rating;

            _context.Feedbacks.Update(existingFeedback);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
