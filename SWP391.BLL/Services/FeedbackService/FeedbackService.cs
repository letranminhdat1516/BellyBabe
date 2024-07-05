using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Feedback;
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

        public async Task<Feedback> CreateFeedbackAsync(int userId, int orderId, int productId, string content, int rating)
        {
            return await _feedbackRepository.CreateFeedbackAsync(userId, orderId, productId, content, rating);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
        }

        public async Task<List<Feedback>> GetFeedbacksByProductIdAsync(int productId)
        {
            return await _feedbackRepository.GetFeedbacksByProductIdAsync(productId);
        }

        public async Task<Feedback> UpdateFeedbackAsync(int feedbackId, string content, int newRating)
        {
            return await _feedbackRepository.UpdateFeedbackAsync(feedbackId, content, newRating);
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }

        public async Task<decimal> GetAverageRatingForProductAsync(int productId)
        {
            return await _feedbackRepository.GetAverageRatingForProductAsync(productId);
        }

        public async Task<List<Feedback>> GetRecentFeedbacksAsync(int count)
        {
            return await _feedbackRepository.GetRecentFeedbacksAsync(count);
        }

        public async Task<bool> CanUserProvideFeedbackAsync(int userId, int productId, int orderDetailId)
        {
            return await _feedbackRepository.CanUserProvideFeedbackAsync(userId, productId, orderDetailId);
        }

        public async Task<List<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
        }

        public async Task<List<FeedbackGroupByRating>> GetFeedbacksByProductIdAndRatingAsync(int productId)
        {
            return await _feedbackRepository.GetFeedbacksByProductIdAndRatingAsync(productId);
        }
    }
}