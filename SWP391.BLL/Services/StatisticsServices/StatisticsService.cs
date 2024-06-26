using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.StatisticsRepository;
using System;

namespace SWP391.BLL.Services
{
    public class StatisticsService
    {
        private readonly StatisticsByWeek _statisticsByWeek;
        private readonly StatisticsByMonth _statisticsByMonth;
        private readonly StatisticsByYear _statisticsByYear;

        public StatisticsService(StatisticsByWeek statisticsByWeek, StatisticsByMonth statisticsByMonth, StatisticsByYear statisticsByYear)
        {
            _statisticsByWeek = statisticsByWeek;
            _statisticsByMonth = statisticsByMonth;
            _statisticsByYear = statisticsByYear;
        }

        public Statistic GetWeeklyStatistics(DateTime startDate, DateTime endDate)
        {
            return _statisticsByWeek.CalculateStatistics(startDate, endDate);
        }

        public Statistic GetMonthlyStatistics(int year, int month)
        {
            return _statisticsByMonth.CalculateStatistics(year, month);
        }

        public Statistic GetYearlyStatistics(int year)
        {
            return _statisticsByYear.CalculateStatistics(year);
        }
    }
}
