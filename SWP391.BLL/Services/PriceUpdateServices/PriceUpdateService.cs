using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.PriceUpdateRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class PriceUpdateService
    {
        private readonly PriceUpdateRepository _priceUpdateRepository;

        public PriceUpdateService(PriceUpdateRepository priceUpdateRepository)
        {
            _priceUpdateRepository = priceUpdateRepository;
        }

        public async Task AddPriceUpdate(decimal? price, DateTime? dateApplied, int? productId)
        {
            await _priceUpdateRepository.AddPriceUpdate(price, dateApplied, productId);
        }

        public async Task DeletePriceUpdate(int priceId)
        {
            await _priceUpdateRepository.DeletePriceUpdate(priceId);
        }

        public async Task UpdatePriceUpdate(int priceId, Dictionary<string, object> updates)
        {
            await _priceUpdateRepository.UpdatePriceUpdate(priceId, updates);
        }

        public async Task<List<PriceUpdate>> GetAllPriceUpdates()
        {
            return await _priceUpdateRepository.GetAllPriceUpdates();
        }

        public async Task<PriceUpdate?> GetPriceUpdateById(int priceId)
        {
            return await _priceUpdateRepository.GetPriceUpdateById(priceId);
        }

        public async Task<List<PriceUpdate>> GetPriceUpdatesByProductId(int productId)
        {
            return await _priceUpdateRepository.GetPriceUpdatesByProductId(productId);
        }
    }
}
