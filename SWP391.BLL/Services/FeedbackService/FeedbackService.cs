using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.FeedbackRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.FeedbackService
{
    public class FeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackService(FeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Feedback> AddFeedbackAsync(int userId, string content, int rating)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("ID người dùng phải là số nguyên dương.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Nội dung không được để trống hoặc chỉ chứa khoảng trắng.", nameof(content));
            }

            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Đánh giá phải từ 1 đến 5.", nameof(rating));
            }

            var feedback = new Feedback
            {
                UserId = userId,
                Content = content,
                Rating = rating
            };

            return await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("ID người dùng phải là số nguyên dương.", nameof(userId));
            }

            return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            if (feedbackId <= 0)
            {
                throw new ArgumentException("ID phản hồi phải là số nguyên dương.", nameof(feedbackId));
            }

            return await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _feedbackRepository.GetAllFeedbacksAsync();
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            if (feedbackId <= 0)
            {
                throw new ArgumentException("ID phản hồi phải là số nguyên dương.", nameof(feedbackId));
            }

            return await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }

        public async Task<bool> UpdateFeedbackAsync(int feedbackId, string content, int rating)
        {
            if (feedbackId <= 0)
            {
                throw new ArgumentException("ID phản hồi phải là số nguyên dương.", nameof(feedbackId));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Nội dung không được để trống hoặc chỉ chứa khoảng trắng.", nameof(content));
            }

            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Đánh giá phải từ 1 đến 5.", nameof(rating));
            }

            var feedback = new Feedback
            {
                FeedbackId = feedbackId,
                Content = content,
                Rating = rating
            };

            return await _feedbackRepository.UpdateFeedbackAsync(feedback);
        }
    }
}