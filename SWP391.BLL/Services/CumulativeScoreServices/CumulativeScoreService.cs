using SWP391.DAL.Repositories.CumulativeScoreRepository;
using System;
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

        public async Task<int> GetUserCumulativeScoreAsync(int userId)
        {
            try
            {
                return await _cumulativeScoreRepository.GetUserCumulativeScoreAsync(userId);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException("Lỗi khi lấy điểm tích lũy của người dùng", ex);
            }
        }

        public async Task AddPointsAsync(int userId, int points)
        {
            if (points < 0)
            {
                throw new ArgumentException("Điểm để thêm phải là số không âm", nameof(points));
            }

            try
            {
                await _cumulativeScoreRepository.AddPointsAsync(userId, points);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException("Lỗi khi thêm điểm tích lũy cho người dùng", ex);
            }
        }

        public async Task UsePointsAsync(int userId, int points)
        {
            if (points < 0)
            {
                throw new ArgumentException("Điểm để dùng phải là số không âm", nameof(points));
            }

            try
            {
                await _cumulativeScoreRepository.UsePointsAsync(userId, points);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException("Lỗi khi dùng điểm tích lũy của người dùng", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException("Không đủ điểm để thực hiện giao dịch", ex);
            }
        }
    }
}
