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
    public class RatingController : ControllerBase
    {
        private readonly RatingService _ratingService;

        public RatingController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost("AddRating")]
        public async Task<IActionResult> AddRating(int? userId, int? productId, int? ratingValue/*, DateTime ratingDate*/)
        {
            try
            {
                await _ratingService.AddRating(userId ?? 0, productId ?? 0, ratingValue ?? 0/*, ratingDate ?? DateTime.Now*/);
                return Ok("Thêm đánh giá thành công");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteRating/{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            try
            {
                var result = await _ratingService.DeleteRating(ratingId);
                if (result)
                {
                    return Ok("Xóa đánh giá thành công");
                }
                return NotFound("Đánh giá không tồn tại");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateRating/{ratingId}")]
        public async Task<IActionResult> UpdateRating(int ratingId, int ratingValue/*, DateTime ratingDate*/)
        {
            try
            {
                var result = await _ratingService.UpdateRating(ratingId, ratingValue/*, ratingDate*/);
                if (result)
                {
                    return Ok("Cập nhật đánh giá thành công");
                }
                return NotFound("Đánh giá không tồn tại");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllRatings")]
        public async Task<ActionResult<List<Rating>>> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllRatings();
            return Ok(ratings);
        }

        [HttpGet("GetRatingById/{ratingId}")]
        public async Task<ActionResult<Rating>> GetRatingById(int ratingId)
        {
            var rating = await _ratingService.GetRatingById(ratingId);
            if (rating == null)
            {
                return NotFound("Đánh giá không tồn tại");
            }
            return Ok(rating);
        }

        [HttpGet("GetRatingsByProductId/{productId}")]
        public async Task<ActionResult<List<Rating>>> GetRatingsByProductId(int productId)
        {
            var ratings = await _ratingService.GetRatingsByProductId(productId);
            return Ok(ratings);
        }

        [HttpGet("GetRatingsByUserId/{userId}")]
        public async Task<ActionResult<List<Rating>>> GetRatingsByUserId(int userId)
        {
            var ratings = await _ratingService.GetRatingsByUserId(userId);
            return Ok(ratings);
        }
    }
}
