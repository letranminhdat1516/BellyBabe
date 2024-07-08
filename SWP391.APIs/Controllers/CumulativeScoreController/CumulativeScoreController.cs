using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.CumulativeScoreServices;
using System;
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

        [HttpGet("GetUserCumulativeScore/{userId}")]
        public async Task<IActionResult> GetUserCumulativeScore(int userId)
        {
            try
            {
                var score = await _cumulativeScoreService.GetUserCumulativeScoreAsync(userId);
                return Ok(score);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{userId}/add/{points}")]
        public async Task<IActionResult> AddPoints(int userId, int points)
        {
            try
            {
                await _cumulativeScoreService.AddPointsAsync(userId, points);
                return Ok($"Thêm {points} điểm thành công cho người dùng {userId}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{userId}/use/{points}")]
        public async Task<IActionResult> UsePoints(int userId, int points)
        {
            try
            {
                await _cumulativeScoreService.UsePointsAsync(userId, points);
                return Ok($"Dùng {points} điểm thành công từ người dùng {userId}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
