using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.DeliveryRepository;
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

        public async Task AddDelivery(string deliveryName, string deliveryMethod, decimal deliveryFee)
        {
            await _deliveryRepository.AddDelivery(deliveryName, deliveryMethod, deliveryFee);
        }

        public async Task DeleteDelivery(int deliveryId)
        {
            await _deliveryRepository.DeleteDelivery(deliveryId);
        }

        public async Task UpdateDelivery(int deliveryId, Dictionary<string, object> updates)
        {
            await _deliveryRepository.UpdateDelivery(deliveryId, updates);
        }

        public async Task<List<Delivery>> GetAllDeliveries()
        {
            return await _deliveryRepository.GetAllDeliveries();
        }

        public async Task<Delivery?> GetDeliveryById(int deliveryId)
        {
            return await _deliveryRepository.GetDeliveryById(deliveryId);
        }
    }
}
