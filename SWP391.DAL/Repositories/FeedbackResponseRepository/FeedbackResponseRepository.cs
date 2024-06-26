using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.FeedbackResponseRepository
{
    public class FeedbackResponseRepository
    {
        private readonly Swp391Context _context;
        private readonly FeedbackRepository.FeedbackRepository _feedbackRepository;

        public FeedbackResponseRepository(Swp391Context context, FeedbackRepository.FeedbackRepository feedbackRepository)
        {
            _context = context;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<FeedbackResponse> AddFeedbackResponseAsync(int feedbackId, int userId, DateTime dateCreated)
        {
            if (feedbackId <= 0)
            {
                throw new ArgumentException("Mã phản hồi không hợp lệ.");
            }

            if (userId <= 0)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            }

            var feedbackExists = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedbackExists == null)
            {
                throw new ArgumentException("Phản hồi không tồn tại.");
            }

            var feedbackResponse = new FeedbackResponse
            {
                FeedbackId = feedbackId,
                UserId = userId,
                DateCreated = dateCreated
            };

            await _context.FeedbackResponses.AddAsync(feedbackResponse);
            await _context.SaveChangesAsync();
            return feedbackResponse;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbackResponsesByFeedbackIdAsync(int feedbackId)
        {
            if (feedbackId <= 0)
            {
                throw new ArgumentException("Mã phản hồi không hợp lệ.");
            }

            return await _context.FeedbackResponses
                .Include(fr => fr.Feedback)
                .Include(fr => fr.User)
                .Where(fr => fr.FeedbackId == feedbackId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbackResponsesByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            }

            return await _context.FeedbackResponses
                .Include(fr => fr.Feedback)
                .Include(fr => fr.User)
                .Where(fr => fr.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FeedbackResponse> GetFeedbackResponseByIdAsync(int responseId)
        {
            if (responseId <= 0)
            {
                throw new ArgumentException("Mã phản hồi không hợp lệ.");
            }

            return await _context.FeedbackResponses
                .Include(fr => fr.Feedback)
                .Include(fr => fr.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(fr => fr.ResponseId == responseId);
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackResponsesAsync()
        {
            return await _context.FeedbackResponses
                .Include(fr => fr.Feedback)
                .Include(fr => fr.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task DeleteFeedbackResponseAsync(int responseId)
        {
            if (responseId <= 0)
            {
                throw new ArgumentException("Mã phản hồi không hợp lệ.");
            }

            var feedbackResponse = await _context.FeedbackResponses.FindAsync(responseId);
            if (feedbackResponse == null)
            {
                throw new ArgumentException("Phản hồi không tồn tại.");
            }

            _context.FeedbackResponses.Remove(feedbackResponse);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedbackResponseAsync(int responseId, int? feedbackId, int? userId, DateTime? dateCreated)
        {
            if (responseId <= 0)
            {
                throw new ArgumentException("Mã phản hồi không hợp lệ.");
            }

            var existingFeedbackResponse = await _context.FeedbackResponses.FindAsync(responseId);
            if (existingFeedbackResponse == null)
            {
                throw new ArgumentException("Phản hồi không tồn tại.");
            }

            if (feedbackId.HasValue)
            {
                if (feedbackId.Value <= 0)
                {
                    throw new ArgumentException("Mã phản hồi không hợp lệ.");
                }

                var feedbackExists = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId.Value);
                if (feedbackExists == null)
                {
                    throw new ArgumentException("Phản hồi không tồn tại.");
                }

                existingFeedbackResponse.FeedbackId = feedbackId.Value;
            }

            if (userId.HasValue)
            {
                if (userId.Value <= 0)
                {
                    throw new ArgumentException("Mã người dùng không hợp lệ.");
                }
                existingFeedbackResponse.UserId = userId.Value;
            }

            if (dateCreated.HasValue)
            {
                existingFeedbackResponse.DateCreated = dateCreated.Value;
            }

            _context.FeedbackResponses.Update(existingFeedbackResponse);
            await _context.SaveChangesAsync();
        }
    }
}
