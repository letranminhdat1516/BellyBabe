using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.OrderStatusRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.OrderStatusServices
{
    public class OrderStatusService
    {
        private readonly OrderStatusRepository _orderStatusRepository;

        public OrderStatusService(OrderStatusRepository orderStatusRepository)
        {
            _orderStatusRepository = orderStatusRepository;
        }

        public async Task<List<OrderStatus>> GetAllOrderStatuses()
        {
            try
            {
                return await _orderStatusRepository.GetAllOrderStatuses();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách trạng thái đơn hàng từ dịch vụ: {ex.Message}");
            }
        }

        public async Task<OrderStatus> GetOrderStatusById(int statusId)
        {
            try
            {
                return await _orderStatusRepository.GetOrderStatusById(statusId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin trạng thái đơn hàng từ dịch vụ: {ex.Message}");
            }
        }

        public async Task AddOrderStatus(OrderStatus orderStatus)
        {
            try
            {
                await _orderStatusRepository.AddOrderStatus(orderStatus);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm trạng thái đơn hàng từ dịch vụ: {ex.Message}");
            }
        }

        public async Task UpdateOrderStatus(int statusId, string statusName)
        {
            try
            {
                await _orderStatusRepository.UpdateOrderStatus(statusId, statusName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái đơn hàng từ dịch vụ: {ex.Message}");
            }
        }

        public async Task DeleteOrderStatus(int statusId)
        {
            try
            {
                await _orderStatusRepository.DeleteOrderStatus(statusId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa trạng thái đơn hàng từ dịch vụ: {ex.Message}");
            }
        }
    }
}
