using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Model.Statistics;
using SWP391.DAL.Services.StatisticsServices;
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

        [HttpGet("OrdersByDateRange")]
        public async Task<IActionResult> GetOrdersByDateRange(string startDate, string endDate)
        {
            try
            {
                var orders = await _statisticsService.GetOrdersByDateRangeAsync(startDate, endDate);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy đơn hàng theo khoảng thời gian: {ex.Message}");
            }
        }

        [HttpGet("WeeklyStatistics")]
        public async Task<IActionResult> GetWeeklyStatistics(string startDate)
        {
            try
            {
                var statistics = await _statisticsService.GetWeeklyStatisticsAsync(startDate);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy thống kê hàng tuần: {ex.Message}");
            }
        }

        [HttpGet("MonthlyStatistics")]
        public async Task<IActionResult> GetMonthlyStatistics(int month, int year)
        {
            try
            {
                var statistics = await _statisticsService.GetMonthlyStatisticsAsync(year, month);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy thống kê hàng tháng: {ex.Message}");
            }
        }

        [HttpGet("YearlyStatistics")]
        public async Task<IActionResult> GetYearlyStatistics(int year)
        {
            try
            {
                var statistics = await _statisticsService.GetYearlyStatisticsAsync(year);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy thống kê hàng năm: {ex.Message}");
            }
        }

        [HttpGet("DailyStatistics")]
        public async Task<IActionResult> GetDailyStatistics(string date)
        {
            try
            {
                var statistics = await _statisticsService.GetDailyStatisticsAsync(date);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy thống kê hàng ngày: {ex.Message}");
            }
        }

        [HttpGet("SalesByCategory")]
        public async Task<IActionResult> GetTotalSalesByCategory()
        {
            try
            {
                var categorySales = await _statisticsService.GetTotalSalesByCategoryAsync();
                if (categorySales == null || categorySales.Count == 0)
                {
                    return NotFound("Không tìm thấy dữ liệu bán hàng cho bất kỳ danh mục nào.");
                }
                return Ok(categorySales);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy tổng doanh số theo danh mục: {ex.Message}");
            }
        }
    }
}