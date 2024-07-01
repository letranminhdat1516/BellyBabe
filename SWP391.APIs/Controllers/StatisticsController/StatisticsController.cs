using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Services.StatisticsServices;
using SWP391.DAL.Model.Statistics;
using System;
using System.Threading.Tasks;

namespace SWP391.DAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyStatistics([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var weeklyStats = await _statisticsService.GetWeeklyStatisticsAsync(startDate, endDate);
            return Ok(weeklyStats);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyStatistics([FromQuery] int month, [FromQuery] int year)
        {
            var monthlyStats = await _statisticsService.GetMonthlyStatisticsAsync(month, year);
            return Ok(monthlyStats);
        }

        [HttpGet("yearly")]
        public async Task<IActionResult> GetYearlyStatistics([FromQuery] int year)
        {
            var yearlyStats = await _statisticsService.GetYearlyStatisticsAsync(year);
            return Ok(yearlyStats);
        }
    }
}
