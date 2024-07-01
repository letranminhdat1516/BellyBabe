using SWP391.DAL.Entities;
using SWP391.DAL.Model.Statistics;
using SWP391.DAL.Repositories.StatisticsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<WeeklyStatistics> GetWeeklyStatisticsAsync(string startDateString, string endDateString)
        {
            DateTime startDate = DateTime.ParseExact(startDateString, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(endDateString, "dd/MM/yyyy", null);

            var orders = await _repository.GetOrdersByDateRangeAsync(startDateString, endDateString);
            return CalculateWeeklyStatistics(orders);
        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(int month, int year)
        {
            var orders = await _repository.GetOrdersByMonthAsync(month, year);
            return CalculateMonthlyStatistics(orders);
        }

        public async Task<YearlyStatistics> GetYearlyStatisticsAsync(int year)
        {
            var orders = await _repository.GetOrdersByYearAsync(year);
            return CalculateYearlyStatistics(orders);
        }

        private WeeklyStatistics CalculateWeeklyStatistics(List<Order> orders)
        {
            var weeklyStats = new WeeklyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count,
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };

            return weeklyStats;
        }

        private MonthlyStatistics CalculateMonthlyStatistics(List<Order> orders)
        {
            var monthlyStats = new MonthlyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count,
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };

            return monthlyStats;
        }

        private YearlyStatistics CalculateYearlyStatistics(List<Order> orders)
        {
            var yearlyStats = new YearlyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count,
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };

            return yearlyStats;
        }

        private List<CategorySales> CalculateCategorySales(List<Order> orders)
        {
            return orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.Product.CategoryId)
                .Select(g => new CategorySales
                {
                    CategoryId = g.Key,
                    TotalSales = g.Sum(od => (od.Price ?? 0) * (od.Quantity ?? 0)),
                    TotalOrders = g.Count()
                })
                .ToList();
        }

        private List<ProductSales> CalculateMostSoldProducts(List<Order> orders)
        {
            return orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.ProductId)
                .Select(g => new ProductSales
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.ProductName ?? "",
                    TotalSold = g.Sum(od => od.Quantity ?? 0),
                    TotalSales = g.Sum(od => (od.Price ?? 0) * (od.Quantity ?? 0))
                })
                .OrderByDescending(p => p.TotalSold)
                .ToList();
        }
    }
}
