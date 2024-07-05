using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.RatingCategoryRepository;

namespace SWP391.BLL.Services.RatingCategoryServices
{
    public class RatingCategoryService
    {
        private readonly RatingCategoryRepository _repository;

        public RatingCategoryService(RatingCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<RatingCategory> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RatingCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<RatingCategory>> GetAllByProductIdAsync(int productId)
        {
            return await _repository.GetAllByProductIdAsync(productId);
        }

        public async Task CreateAsync(RatingCategory ratingCategory)
        {
            if (string.IsNullOrWhiteSpace(ratingCategory.CategoryName))
            {
                throw new ArgumentException("Category name cannot be empty");
            }

            await _repository.CreateAsync(ratingCategory);
        }

        public async Task UpdateAsync(RatingCategory ratingCategory)
        {
            if (string.IsNullOrWhiteSpace(ratingCategory.CategoryName))
            {
                throw new ArgumentException("Category name cannot be empty");
            }

            await _repository.UpdateAsync(ratingCategory);
        }

        public async Task DeleteAsync(int id)
        {
            var ratingCategory = await _repository.GetByIdAsync(id);
            if (ratingCategory == null)
            {
                throw new KeyNotFoundException("Rating category not found");
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<double> CalculateAverageRatingByProductIdAsync(int productId)
        {
            return await _repository.CalculateAverageRatingByProductIdAsync(productId);
        }
    }
}