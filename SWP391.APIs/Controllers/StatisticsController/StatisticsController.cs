using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using System;

namespace SWP391.API.Controllers
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
        public IActionResult GetWeeklyStatistics(DateTime startDate, DateTime endDate)
        {
            var statistics = _statisticsService.GetWeeklyStatistics(startDate, endDate);
            return Ok(statistics);
        }

        [HttpGet("monthly")]
        public IActionResult GetMonthlyStatistics(int year, int month)
        {
            var statistics = _statisticsService.GetMonthlyStatistics(year, month);
            return Ok(statistics);
        }

        [HttpGet("yearly")]
        public IActionResult GetYearlyStatistics(int year)
        {
            var statistics = _statisticsService.GetYearlyStatistics(year);
            return Ok(statistics);
        }
    }
}
