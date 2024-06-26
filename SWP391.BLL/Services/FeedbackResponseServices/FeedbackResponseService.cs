using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.FeedbackResponseRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class FeedbackResponseService
    {
        private readonly FeedbackResponseRepository _feedbackResponseRepository;

        public FeedbackResponseService(FeedbackResponseRepository feedbackResponseRepository)
        {
            _feedbackResponseRepository = feedbackResponseRepository ?? throw new ArgumentNullException(nameof(feedbackResponseRepository));
        }

        public async Task<FeedbackResponse> AddFeedbackResponseAsync(int feedbackId, int userId, DateTime dateCreated)
        {
            try
            {
                return await _feedbackResponseRepository.AddFeedbackResponseAsync(feedbackId, userId, dateCreated);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Thêm phản hồi thất bại: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbackResponsesByFeedbackIdAsync(int feedbackId)
        {
            try
            {
                return await _feedbackResponseRepository.GetFeedbackResponsesByFeedbackIdAsync(feedbackId);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Lấy phản hồi theo mã phản hồi thất bại: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbackResponsesByUserIdAsync(int userId)
        {
            try
            {
                return await _feedbackResponseRepository.GetFeedbackResponsesByUserIdAsync(userId);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Lấy phản hồi theo mã người dùng thất bại: {ex.Message}", ex);
            }
        }

        public async Task<FeedbackResponse> GetFeedbackResponseByIdAsync(int responseId)
        {
            try
            {
                return await _feedbackResponseRepository.GetFeedbackResponseByIdAsync(responseId);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Lấy phản hồi theo mã phản hồi thất bại: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackResponsesAsync()
        {
            return await _feedbackResponseRepository.GetAllFeedbackResponsesAsync();
        }

        public async Task DeleteFeedbackResponseAsync(int responseId)
        {
            try
            {
                await _feedbackResponseRepository.DeleteFeedbackResponseAsync(responseId);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Xóa phản hồi thất bại: {ex.Message}", ex);
            }
        }

        public async Task UpdateFeedbackResponseAsync(int responseId, int? feedbackId, int? userId, DateTime? dateCreated)
        {
            try
            {
                await _feedbackResponseRepository.UpdateFeedbackResponseAsync(responseId, feedbackId, userId, dateCreated);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Cập nhật phản hồi thất bại: {ex.Message}", ex);
            }
        }
    }
}
