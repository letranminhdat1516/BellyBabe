using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.FeedbackRepository;

namespace SWP391.BLL.Services
{
    public class FeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackService(FeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Feedback> CreateFeedbackAsync(int userId, int productId, string content, int rating)
        {
            return await _feedbackRepository.CreateFeedbackAsync(userId, productId, content, rating);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
        }

        public async Task<List<Feedback>> GetFeedbacksByProductIdAsync(int productId)
        {
            return await _feedbackRepository.GetFeedbacksByProductIdAsync(productId);
        }

        public async Task<Feedback> UpdateFeedbackAsync(int feedbackId, string content, int rating)
        {
            return await _feedbackRepository.UpdateFeedbackAsync(feedbackId, content, rating);
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }

        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            return await _feedbackRepository.GetAverageRatingForProductAsync(productId);
        }

        public async Task<List<Feedback>> GetRecentFeedbacksAsync(int count)
        {
            return await _feedbackRepository.GetRecentFeedbacksAsync(count);
        }

        public async Task<bool> HasUserPurchasedProductAsync(int userId, int productId)
        {
            return await _feedbackRepository.HasUserPurchasedProductAsync(userId, productId);
        }

        public async Task<List<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
        }
    }
}