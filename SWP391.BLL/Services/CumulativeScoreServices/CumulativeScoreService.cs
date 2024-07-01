using SWP391.DAL.Repositories.CumulativeScoreRepository;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.CumulativeScoreServices
{
    public class CumulativeScoreService
    {
        private readonly CumulativeScoreRepository _cumulativeScoreRepository;

        public CumulativeScoreService(CumulativeScoreRepository cumulativeScoreRepository)
        {
            _cumulativeScoreRepository = cumulativeScoreRepository;
        }

        public async Task UpdateCumulativeScoreAsync(int userId)
        {
            await _cumulativeScoreRepository.UpdateCumulativeScoreAsync(userId);
        }

        public async Task<int?> GetCumulativeScoreAsync(int userId)
        {
            return await _cumulativeScoreRepository.GetCumulativeScoreAsync(userId);
        }

        public async Task UpdateOrderScoreAsync(int orderId)
        {
            await _cumulativeScoreRepository.UpdateOrderScoreAsync(orderId);
        }
    }
}