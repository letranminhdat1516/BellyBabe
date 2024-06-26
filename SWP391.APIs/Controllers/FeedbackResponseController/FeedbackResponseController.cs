using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackResponseController : ControllerBase
    {
        private readonly FeedbackResponseService _feedbackResponseService;

        public FeedbackResponseController(FeedbackResponseService feedbackResponseService)
        {
            _feedbackResponseService = feedbackResponseService ?? throw new ArgumentNullException(nameof(feedbackResponseService));
        }

        [HttpPost("AddFeedbackResponse")]
        public async Task<IActionResult> AddFeedbackResponse(int feedbackId, int userId, DateTime dateCreated)
        {
            try
            {
                var feedbackResponse = await _feedbackResponseService.AddFeedbackResponseAsync(feedbackId, userId, dateCreated);
                return CreatedAtAction(nameof(GetFeedbackResponseByIdAsync), new { responseId = feedbackResponse.ResponseId }, feedbackResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = $"Thêm phản hồi không thành công: {ex.Message}" });
            }
        }

        [HttpGet("GetFeedbackById/{feedbackId}")]
        public async Task<IActionResult> GetFeedbackResponsesByFeedbackIdAsync(int feedbackId)
        {
            try
            {
                var feedbackResponses = await _feedbackResponseService.GetFeedbackResponsesByFeedbackIdAsync(feedbackId);
                return Ok(feedbackResponses);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = $"Lấy danh sách phản hồi theo mã phản hồi {feedbackId} không thành công: {ex.Message}" });
            }
        }

        [HttpGet("GetFeedbackResponseByUserId/{userId}")]
        public async Task<IActionResult> GetFeedbackResponsesByUserIdAsync(int userId)
        {
            try
            {
                var feedbackResponses = await _feedbackResponseService.GetFeedbackResponsesByUserIdAsync(userId);
                return Ok(feedbackResponses);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = $"Lấy danh sách phản hồi theo mã người dùng {userId} không thành công: {ex.Message}" });
            }
        }

        [HttpGet("GetFeedbackResponseById/{responseId}")]
        public async Task<IActionResult> GetFeedbackResponseByIdAsync(int responseId)
        {
            try
            {
                var feedbackResponse = await _feedbackResponseService.GetFeedbackResponseByIdAsync(responseId);
                if (feedbackResponse == null)
                    return NotFound(new { message = $"Không tìm thấy phản hồi với mã {responseId}" });

                return Ok(feedbackResponse);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = $"Lấy phản hồi theo mã {responseId} không thành công: {ex.Message}" });
            }
        }

        [HttpGet("GetAllFeedbacks")]
        public async Task<IActionResult> GetAllFeedbackResponsesAsync()
        {
            var feedbackResponses = await _feedbackResponseService.GetAllFeedbackResponsesAsync();
            return Ok(feedbackResponses);
        }

        [HttpDelete("DeleteFeedbackResponse/{responseId}")]
        public async Task<IActionResult> DeleteFeedbackResponseAsync(int responseId)
        {
            try
            {
                await _feedbackResponseService.DeleteFeedbackResponseAsync(responseId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = $"Xóa phản hồi với mã {responseId} không thành công: {ex.Message}" });
            }
        }

        [HttpPut("UpdateFeedbackResponse/{responseId}")]
        public async Task<IActionResult> UpdateFeedbackResponseAsync(int responseId, int? feedbackId, int? userId, DateTime? dateCreated)
        {
            try
            {
                await _feedbackResponseService.UpdateFeedbackResponseAsync(responseId, feedbackId, userId, dateCreated);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = $"Cập nhật phản hồi với mã {responseId} không thành công: {ex.Message}" });
            }
        }
    }
}
