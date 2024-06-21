using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.CumulativeScoreServices;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CumulativeScoreController : ControllerBase
    {
        private readonly CumulativeScoreService _cumulativeScoreService;

        public CumulativeScoreController(CumulativeScoreService cumulativeScoreService)
        {
            _cumulativeScoreService = cumulativeScoreService;
        }

        [HttpPost("updateScore/{userId}")]
        public async Task<IActionResult> UpdateCumulativeScoreAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            await _cumulativeScoreService.UpdateCumulativeScoreAsync(userId);
            return Ok("Cumulative score updated successfully.");
        }

        [HttpGet("getScore/{userId}")]
        public async Task<IActionResult> GetCumulativeScoreAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID must be provided.");
            }

            var score = await _cumulativeScoreService.GetCumulativeScoreAsync(userId);
            if (score == null)
            {
                return NotFound("No cumulative score found for this user.");
            }

            return Ok(score);
        }
    }
}
