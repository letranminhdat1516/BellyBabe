using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using SWP391.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RatingService _ratingService;

        public RatingController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost("AddRating")]
        public async Task<IActionResult> AddRating(int? userId, int? productId, int? ratingValue, DateTime? ratingDate)
        {
            var result = await _ratingService.AddRating(userId, productId, ratingValue, ratingDate);
            if (!result)
            {
                return BadRequest("User has not purchased the product or the order has not been delivered.");
            }
            return Ok("Add Rating successfully");
        }

        [HttpDelete("DeleteRating/{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            var result = await _ratingService.DeleteRating(ratingId);
            if (!result)
            {
                return NotFound("Rating not found.");
            }
            return Ok();
        }

        [HttpPut("UpdateRating/{ratingId}")]
        public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] Dictionary<string, object> updates)
        {
            var result = await _ratingService.UpdateRating(ratingId, updates);
            if (!result)
            {
                return NotFound("Rating not found.");
            }
            return Ok();
        }

        [HttpGet("GetAllRatings")]
        public async Task<ActionResult<List<Rating>>> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllRatings();
            return Ok(ratings);
        }

        [HttpGet("GetRatingsById/{ratingId}")]
        public async Task<ActionResult<Rating?>> GetRatingById(int ratingId)
        {
            var rating = await _ratingService.GetRatingById(ratingId);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpGet("GetRatingsByProduct/{productId}")]
        public async Task<ActionResult<List<Rating>>> GetRatingsByProductId(int productId)
        {
            var ratings = await _ratingService.GetRatingsByProductId(productId);
            return Ok(ratings);
        }

        [HttpGet("GetRatingsByUser/{userId}")]
        public async Task<ActionResult<List<Rating>>> GetRatingsByUserId(int userId)
        {
            var ratings = await _ratingService.GetRatingsByUserId(userId);
            return Ok(ratings);
        }
    }
}
