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
            _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
        }

        public async Task<Feedback> AddFeedbackAsync(int userId, string content, int rating)
        {
            try
            {
                return await _feedbackRepository.AddFeedbackAsync(userId, content, rating);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Thêm phản hồi thất bại: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            try
            {
                return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy phản hồi theo người dùng thất bại: {ex.Message}");
            }
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
                if (feedback == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy phản hồi.");
                }
                return feedback;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Không tìm thấy phản hồi.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy thông tin phản hồi thất bại: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            try
            {
                return await _feedbackRepository.GetAllFeedbacksAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy danh sách phản hồi thất bại: {ex.Message}");
            }
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            try
            {
                return await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Xóa phản hồi thất bại: {ex.Message}");
            }
        }

        public async Task<bool> UpdateFeedbackAsync(int feedbackId, string content, int rating)
        {
            try
            {
                return await _feedbackRepository.UpdateFeedbackAsync(feedbackId, content, rating);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Cập nhật phản hồi thất bại: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Cập nhật phản hồi thất bại: {ex.Message}");
            }
        }
    }
}
