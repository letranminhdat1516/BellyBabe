using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.FeedbackService;
using SWP391.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers.FeedbackController
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback(int userId, string content, int rating)
        {
            if (userId <= 0 || string.IsNullOrEmpty(content) || rating < 1 || rating > 5)
            {
                return BadRequest("Phản hồi phải chứa ID người dùng hợp lệ, nội dung và đánh giá từ 1 đến 5.");
            }

            try
            {
                var addedFeedback = await _feedbackService.AddFeedbackAsync(userId, content, rating);
                return CreatedAtAction(nameof(GetFeedbackById), new { feedbackId = addedFeedback.FeedbackId }, addedFeedback);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFeedbacksByUserId(int userId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
            return Ok(feedbacks);
        }

        [HttpGet("{feedbackId}")]
        public async Task<IActionResult> GetFeedbackById(int feedbackId)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null)
            {
                return NotFound("Không tìm thấy phản hồi");
            }
            return Ok(feedback);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
            if (!result)
            {
                return NotFound("Không tìm thấy phản hồi để xóa");
            }
            return Ok("Đã xóa phản hồi thành công");
        }

        [HttpPut("{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, string content, int rating)
        {
            if (string.IsNullOrEmpty(content) || rating < 1 || rating > 5)
            {
                return BadRequest("Phải cung cấp nội dung hợp lệ và đánh giá từ 1 đến 5 để cập nhật.");
            }

            try
            {
                var result = await _feedbackService.UpdateFeedbackAsync(feedbackId, content, rating);
                if (!result)
                {
                    return NotFound("Không tìm thấy phản hồi để cập nhật");
                }
                return Ok("Đã cập nhật phản hồi thành công");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}