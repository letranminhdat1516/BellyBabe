using System;
using System.Globalization;
using System.Linq;
using SWP391.DAL.Swp391DbContext;
using SWP391.DAL.Entities;

namespace SWP391.DAL.Repositories.StatisticsRepository
{
    public class StatisticsByYear
    {
        private readonly Swp391Context _context;

        public StatisticsByYear(Swp391Context context)
        {
            _context = context;
        }

        public Statistic CalculateStatistics(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1).AddDays(-1);

            var orders = _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToList();

            var totalOrders = orders.Count;
            var totalItemsSold = orders.Sum(o => o.OrderDetails.Sum(od => od.Quantity)) ?? 0;
            var totalAmount = orders.Sum(o => o.TotalPrice ?? 0);
            var profit = CalculateProfit(orders);

            var formattedStartDate = startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            var statistics = new Statistic
            {
                Date = formattedStartDate,
                NumberOfOrders = totalOrders,
                ItemsSold = totalItemsSold,
                TotalAmount = totalAmount,
                Profit = profit
            };

            return statistics;
        }

        private decimal CalculateProfit(List<Order> orders)
        {
            return orders.Sum(o => (o.TotalPrice ?? 0) - o.OrderDetails.Sum(od => od.Price ?? 0));
        }
    }
}
