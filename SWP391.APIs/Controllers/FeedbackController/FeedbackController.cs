using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.BLL.Services;

namespace SWP391.Controllers
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

        [HttpPost("AddFeedback")]
        public async Task<IActionResult> CreateFeedback(int userId, int orderId, int productId, string content, int rating)
        {
            try
            {
                var feedback = await _feedbackService.CreateFeedbackAsync(userId, orderId, productId, content, rating);
                return Ok(feedback);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu của bạn.", error = ex.Message });
            }
        }

        [HttpGet("GetFeedbackById/{id}")]
        public async Task<IActionResult> GetFeedback(int id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }

        [HttpGet("GetFeedbackByProductId/{productId}")]
        public async Task<IActionResult> GetFeedbacksByProduct(int productId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByProductIdAsync(productId);
            return Ok(feedbacks);
        }

        [HttpPut("UpdateFeedback/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, string content, int newRating)
        {
            try
            {
                var feedback = await _feedbackService.UpdateFeedbackAsync(id, content, newRating);
                return Ok(feedback);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteFeedback/{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                await _feedbackService.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //[HttpGet("AverageRating/{productId}")]
        //public async Task<IActionResult> GetAverageRating(int productId)
        //{
        //    var averageRating = await _feedbackService.GetAverageRatingForProductAsync(productId);
        //    return Ok(averageRating);
        //}

        [HttpGet("RecentFeedbacks/{count}")]
        public async Task<IActionResult> GetRecentFeedbacks(int count)
        {
            var recentFeedbacks = await _feedbackService.GetRecentFeedbacksAsync(count);
            return Ok(recentFeedbacks);
        }

        //[HttpGet("user/{userId}/can-provide-feedback/{productId}/{orderDetailId}")]
        //public async Task<IActionResult> CanUserProvideFeedback(int userId, int productId, int orderDetailId)
        //{
        //    var canProvideFeedback = await _feedbackService.CanUserProvideFeedbackAsync(userId, productId, orderDetailId);
        //    return Ok(canProvideFeedback);
        //}

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFeedbacksByUser(int userId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
            return Ok(feedbacks);
        }

        [HttpGet("GetFeedbackByProductIdAndRating/{productId}")]
        public async Task<IActionResult> GetFeedbacksByProductIdAndRating(int productId)
        {
            var groupedFeedbacks = await _feedbackService.GetFeedbacksByProductIdAndRatingAsync(productId);
            return Ok(groupedFeedbacks);
        }
    }
}