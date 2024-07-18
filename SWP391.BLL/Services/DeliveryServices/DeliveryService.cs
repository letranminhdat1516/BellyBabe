using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.DeliveryRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class DeliveryService
    {
        private readonly DeliveryRepository _deliveryRepository;

        public DeliveryService(DeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task AddDelivery(string deliveryName, int? deliveryFee)
        {
            try
            {
                await _deliveryRepository.AddDelivery(deliveryName, deliveryFee);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Thêm phương thức giao hàng thất bại: {ex.Message}");
            }
        }

        public async Task DeleteDelivery(int deliveryId)
        {
            try
            {
                await _deliveryRepository.DeleteDelivery(deliveryId);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Xóa phương thức giao hàng thất bại: Không tìm thấy phương thức giao hàng.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Xóa phương thức giao hàng thất bại: {ex.Message}");
            }
        }

        public async Task UpdateDelivery(int deliveryId, string? deliveryName, int? deliveryFee)
        {
            try
            {
                await _deliveryRepository.UpdateDelivery(deliveryId, deliveryName, deliveryFee);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Cập nhật phương thức giao hàng thất bại: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Cập nhật phương thức giao hàng thất bại: {ex.Message}");
            }
        }

        public async Task<List<DeliveryMethod>> GetAllDeliveries()
        {
            try
            {
                return await _deliveryRepository.GetAllDeliveries();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy danh sách phương thức giao hàng thất bại: {ex.Message}");
            }
        }

        public async Task<DeliveryMethod> GetDeliveryById(int deliveryId)
        {
            try
            {
                return await _deliveryRepository.GetDeliveryById(deliveryId) ?? throw new KeyNotFoundException("Không tìm thấy phương thức giao hàng.");
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Không tìm thấy phương thức giao hàng.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy thông tin phương thức giao hàng thất bại: {ex.Message}");
            }
        }
    }
}
