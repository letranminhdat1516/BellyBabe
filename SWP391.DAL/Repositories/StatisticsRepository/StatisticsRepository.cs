   using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Swp391DbContext;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.StatisticsRepository
{
    public class StatisticsRepository
    {
        private readonly Swp391Context _context;

        public StatisticsRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByDateRangeAsync(string startDateString, string endDateString)
        {
            DateTime startDate = DateTime.ParseExact(startDateString, "dd/MM/yyyy", null);
            DateTime endDate = DateTime.ParseExact(endDateString, "dd/MM/yyyy", null);
            var deliveredStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã giao hàng");

            if (deliveredStatus == null)
            {
                throw new ArgumentException($"Status 'Đã giao hàng' not found.");
            }

            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.OrderStatuses)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate &&
                            o.OrderStatuses.Any(os => os.StatusId == deliveredStatus.StatusId))
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByYearAsync(int year)
        {
            var deliveredStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã giao hàng");

            if (deliveredStatus == null)
            {
                throw new ArgumentException($"Status 'Đã giao hàng' not found.");
            }

            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.OrderStatuses)
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == year &&
                            o.OrderStatuses.Any(os => os.StatusId == deliveredStatus.StatusId))
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByMonthAsync(int month, int year)
        {
            var deliveredStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Đã giao hàng");

            if (deliveredStatus == null)
            {
                throw new ArgumentException($"Status 'Đã giao hàng' not found.");
            }

            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.OrderStatuses)
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == month && o.OrderDate.Value.Year == year &&
                            o.OrderStatuses.Any(os => os.StatusId == deliveredStatus.StatusId))
                .ToListAsync();
        }
    }
}
