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
        public async Task<IActionResult> CreateFeedback(int userId, int productId, string content, int rating)
        {
            try
            {
                var feedback = await _feedbackService.CreateFeedbackAsync(userId, productId, content, rating);
                return CreatedAtAction(nameof(GetFeedback), new { id = feedback.FeedbackId }, feedback);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
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
        public async Task<IActionResult> UpdateFeedback(int id, string content, int rating)
        {
            try
            {
                var feedback = await _feedbackService.UpdateFeedbackAsync(id, content, rating);
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

        [HttpGet("user/{userId}/has-purchased/{productId}")]
        public async Task<IActionResult> HasUserPurchasedProduct(int userId, int productId)
        {
            var hasPurchased = await _feedbackService.HasUserPurchasedProductAsync(userId, productId);
            return Ok(hasPurchased);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFeedbacksByUser(int userId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
            return Ok(feedbacks);
        }
    }
}