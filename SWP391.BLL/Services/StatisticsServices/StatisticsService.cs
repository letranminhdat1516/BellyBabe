using SWP391.DAL.Entities;
using SWP391.DAL.Model.Statistics;
using SWP391.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Services.StatisticsServices
{
    public class StatisticsService
    {
        private readonly StatisticsRepository _repository;

        public StatisticsService(StatisticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Order>> GetOrdersByDateRangeAsync(string startDateString, string endDateString)
        {
            return await _repository.GetOrdersByDateRangeAsync(startDateString, endDateString);
        }

        public async Task<WeeklyStatistics> GetWeeklyStatisticsAsync(string startDateString)
        {
            return await _repository.GetOrdersForWeekAsync(startDateString);
        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(int month, int year)
        {
            return await _repository.GetOrdersByMonthAsync(year, month);
        }

        public async Task<YearlyStatistics> GetYearlyStatisticsAsync(int year)
        {
            return await _repository.GetOrdersByYearAsync(year);
        }

        public async Task<List<CategorySales>> GetTotalSalesByCategoryAsync()
        {
            return await _repository.GetTotalSalesByCategoryAsync();
        }
    }
}
