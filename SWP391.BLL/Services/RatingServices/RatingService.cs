using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.RatingRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class RatingService
    {
        private readonly RatingRepository _ratingRepository;

        public RatingService(RatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task AddRating(int userId, int productId, int ratingValue/*, DateTime ratingDate*/)
        {
            await _ratingRepository.AddRating(userId, productId, ratingValue/*, ratingDate*/);
        }

        public async Task<bool> DeleteRating(int ratingId)
        {
            return await _ratingRepository.DeleteRating(ratingId);
        }

        public async Task<bool> UpdateRating(int ratingId, int ratingValue/*, DateTime ratingDate*/)
        {
            return await _ratingRepository.UpdateRating(ratingId, ratingValue/*, ratingDate*/);
        }

        public async Task<List<Rating>> GetAllRatings()
        {
            return await _ratingRepository.GetAllRatings();
        }

        public async Task<Rating?> GetRatingById(int ratingId)
        {
            return await _ratingRepository.GetRatingById(ratingId);
        }

        public async Task<List<Rating>> GetRatingsByProductId(int productId)
        {
            return await _ratingRepository.GetRatingsByProductId(productId);
        }

        public async Task<List<Rating>> GetRatingsByUserId(int userId)
        {
            return await _ratingRepository.GetRatingsByUserId(userId);
        }
    }
}
