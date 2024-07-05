using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SWP391.BLL.Services.RatingCategoryServices;
using SWP391.DAL.Entities;

namespace SWP391.APIs.Controllers.RatingCategoryController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingCategoryController : ControllerBase
    {
        private readonly RatingCategoryService _service;

        public RatingCategoryController(RatingCategoryService service)
        {
            _service = service;
        }

        [HttpGet("GetRatingCategoryById/{ratingCategoryId}")]
        public async Task<ActionResult<RatingCategory>> GetById(int ratingCategoryId)
        {
            var ratingCategory = await _service.GetByIdAsync(ratingCategoryId);
            if (ratingCategory == null)
            {
                return NotFound();
            }
            return Ok(ratingCategory);
        }

        [HttpGet("GetAllRatingCategories")]
        public async Task<ActionResult<IEnumerable<RatingCategory>>> GetAll()
        {
            var ratingCategories = await _service.GetAllAsync();
            return Ok(ratingCategories);
        }

        [HttpGet("GetAllRatingCategoriesByProductID/{productId}")]
        public async Task<ActionResult<IEnumerable<RatingCategory>>> GetAllByProductId(int productId)
        {
            var ratingCategories = await _service.GetAllByProductIdAsync(productId);
            return Ok(ratingCategories);
        }

        //[HttpPost("CreateRatingCategory")]
        //public async Task<ActionResult<RatingCategory>> Create(RatingCategory ratingCategory)
        //{
        //    try
        //    {
        //        await _service.CreateAsync(ratingCategory);
        //        return CreatedAtAction(nameof(GetById), new { id = ratingCategory.CategoryId }, ratingCategory);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut("UpdateRatingCategory/{ratingCategoryId}")]
        public async Task<IActionResult> Update(int ratingCategoryId, RatingCategory ratingCategory)
        {
            if (ratingCategoryId != ratingCategory.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateAsync(ratingCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("DeleteRatingCategoryById/{ratingCategoryId}")]
        public async Task<IActionResult> Delete(int ratingCategoryId)
        {
            try
            {
                await _service.DeleteAsync(ratingCategoryId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("CalculateAverageRating/{productId}")]
        public async Task<IActionResult> CalculateAverageRating(int productId)
        {
            var averageRating = await _service.CalculateAverageRatingByProductIdAsync(productId);
            return Ok(averageRating);
        }
    }
}