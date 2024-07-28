using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Statistics;
using SWP391.DAL.Swp391DbContext;

namespace SWP391.DAL.Repositories
{
    public class StatisticsRepository
    {
        private readonly Swp391Context _context;

        public StatisticsRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByDateRangeAsync(string startDate, string endDate)
        {
            DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            return await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.StatusId == 6 && o.OrderDate >= start && o.OrderDate <= end)
                .ToListAsync();
        }

        public async Task<WeeklyStatistics> GetOrdersForWeekAsync(string startDate)
        {
            DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end = start.AddDays(6);

            var orders = await GetOrdersByDateRangeAsync(startDate, end.ToString("dd/MM/yyyy"));

            return CalculateWeeklyStatistics(orders);
        }

        public async Task<MonthlyStatistics> GetOrdersByMonthAsync(int year, int month)
        {
            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.StatusId == 6 && o.OrderDate.HasValue && o.OrderDate.Value.Year == year && o.OrderDate.Value.Month == month)
                .ToListAsync();

            return CalculateMonthlyStatistics(orders);
        }

        public async Task<YearlyStatistics> GetOrdersByYearAsync(int year)
        {
            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.StatusId == 6 && o.OrderDate.HasValue && o.OrderDate.Value.Year == year)
                .ToListAsync();

            return CalculateYearlyStatistics(orders);
        }

        public async Task<DailyStatistics> GetOrdersByDayAsync(string date)
        {
            DateTime day = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.StatusId == 6 && o.OrderDate.HasValue && o.OrderDate.Value.Date == day.Date)
                .ToListAsync();

            return CalculateDailyStatistics(orders);
        }

        public async Task<List<CategorySales>> GetTotalSalesByCategoryAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.StatusId == 6)
                .ToListAsync();

            return orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.Product.CategoryId)
                .Select(g => new CategorySales
                {
                    CategoryId = g.Key,
                    CategoryName = g.First().Product.Category?.CategoryName ?? "Unknown",
                    TotalSales = g.Sum(od => od.Price ?? 0),
                    TotalOrders = g.Count()  
                })
                .ToList();
        }

        private WeeklyStatistics CalculateWeeklyStatistics(List<Order> orders)
        {
            return new WeeklyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count, 
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };
        }

        private MonthlyStatistics CalculateMonthlyStatistics(List<Order> orders)
        {
            return new MonthlyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count, 
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };
        }

        private YearlyStatistics CalculateYearlyStatistics(List<Order> orders)
        {
            return new YearlyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count, 
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };
        }

        private DailyStatistics CalculateDailyStatistics(List<Order> orders)
        {
            return new DailyStatistics
            {
                TotalSales = orders.Sum(o => o.TotalPrice ?? 0),
                TotalOrders = orders.Count,
                CategorySales = CalculateCategorySales(orders),
                MostSoldProducts = CalculateMostSoldProducts(orders)
            };
        }

        private List<CategorySales> CalculateCategorySales(List<Order> orders)
        {
            return orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.Product.CategoryId)
                .Select(g => new CategorySales
                {
                    CategoryId = g.Key,
                    CategoryName = g.First().Product.Category?.CategoryName ?? "Unknown",
                    TotalSales = g.Sum(od => od.Price ?? 0),
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
                    ProductName = g.First().Product?.ProductName ?? "Unknown",
                    TotalSold = g.Sum(od => od.Quantity ?? 0),
                    TotalSales = g.Sum(od => od.Price ?? 0)
                })
                .OrderByDescending(ps => ps.TotalSold)
                .ToList();
        }
    }
}
