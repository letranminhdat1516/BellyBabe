using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.OrderStatusRepository
{
    public class OrderStatusRepository
    {
        private readonly Swp391Context _context;

        public OrderStatusRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<List<OrderStatus>> GetAllOrderStatuses()
        {
            try
            {
                return await _context.OrderStatuses
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách trạng thái đơn hàng: {ex.Message}");
            }
        }

        public async Task<OrderStatus> GetOrderStatusById(int statusId)
        {
            try
            {
                var orderStatus = await _context.OrderStatuses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(os => os.StatusId == statusId);

                if (orderStatus == null)
                {
                    throw new ArgumentException("Không tìm thấy trạng thái đơn hàng này.");
                }

                return orderStatus;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin trạng thái đơn hàng: {ex.Message}");
            }
        }

        public async Task AddOrderStatus(OrderStatus orderStatus)
        {
            try
            {
                if (orderStatus == null)
                {
                    throw new ArgumentNullException(nameof(orderStatus), "Thông tin trạng thái đơn hàng không được để trống.");
                }

                _context.OrderStatuses.Add(orderStatus);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Đã xảy ra lỗi khi thêm trạng thái đơn hàng vào cơ sở dữ liệu. Vui lòng thử lại sau.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Đã xảy ra lỗi khi thêm trạng thái đơn hàng: {ex.Message}");
            }
        }

        public async Task UpdateOrderStatus(int statusId, string statusName)
        {
            try
            {
                var existingOrderStatus = await _context.OrderStatuses.FindAsync(statusId);
                if (existingOrderStatus == null)
                {
                    throw new ArgumentException("Không tìm thấy trạng thái đơn hàng để cập nhật.");
                }

                existingOrderStatus.StatusName = statusName;

                await _context.SaveChangesAsync();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng vào cơ sở dữ liệu. Vui lòng thử lại sau.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng: {ex.Message}");
            }
        }


        public async Task DeleteOrderStatus(int statusId)
        {
            try
            {
                var orderStatus = await _context.OrderStatuses.FindAsync(statusId);

                if (orderStatus == null)
                {
                    throw new ArgumentException("Không tìm thấy trạng thái đơn hàng để xóa.");
                }

                _context.OrderStatuses.Remove(orderStatus);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Đã xảy ra lỗi khi xóa trạng thái đơn hàng từ cơ sở dữ liệu. Vui lòng thử lại sau.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Đã xảy ra lỗi khi xóa trạng thái đơn hàng: {ex.Message}");
            }
        }
    }
}
