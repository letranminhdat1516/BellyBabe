using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.FeedbackService;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService ?? throw new ArgumentNullException(nameof(feedbackService));
        }

        [HttpPost("AddFeedback")]
        public async Task<IActionResult> AddFeedback(int userId, string content, int rating)
        {
            try
            {
                var feedback = await _feedbackService.AddFeedbackAsync(userId, content, rating);
                return CreatedAtAction(nameof(GetFeedbackById), new { feedbackId = feedback.FeedbackId }, feedback);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpGet("GetFeedbacksByUserId/{userId}")]
        public async Task<IActionResult> GetFeedbacksByUserId(int userId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpGet("GetFeedbackById/{feedbackId}")]
        public async Task<IActionResult> GetFeedbackById(int feedbackId)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackId);
                if (feedback == null)
                {
                    return NotFound("Không tìm thấy phản hồi.");
                }
                return Ok(feedback);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Không tìm thấy phản hồi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpGet("GetAllFeedbacks")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpDelete("DeleteFeedback/{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
                if (!result)
                {
                    return NotFound("Không tìm thấy phản hồi để xóa.");
                }
                return Ok("Xóa phản hồi thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }

        [HttpPut("UpdateFeedback/{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, string content, int rating)
        {
            try
            {
                var result = await _feedbackService.UpdateFeedbackAsync(feedbackId, content, rating);
                if (!result)
                {
                    return NotFound("Không tìm thấy phản hồi để cập nhật.");
                }
                return Ok("Cập nhật phản hồi thành công.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}
