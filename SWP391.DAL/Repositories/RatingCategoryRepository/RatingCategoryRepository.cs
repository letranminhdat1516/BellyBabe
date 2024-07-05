using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;

namespace SWP391.DAL.Repositories.RatingCategoryRepository
{
    public class RatingCategoryRepository
    {
        private readonly Swp391Context _context;

        public RatingCategoryRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<RatingCategory> GetByIdAsync(int id)
        {
            return await _context.RatingCategories.FindAsync(id);
        }

        public async Task<IEnumerable<RatingCategory>> GetAllAsync()
        {
            return await _context.RatingCategories.ToListAsync();
        }

        public async Task<IEnumerable<RatingCategory>> GetAllByProductIdAsync(int productId)
        {
            return await _context.RatingCategories
                .Where(rc => rc.ProductId == productId)
                .ToListAsync();
        }

        public async Task CreateAsync(RatingCategory ratingCategory)
        {
            await _context.RatingCategories.AddAsync(ratingCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RatingCategory ratingCategory)
        {
            _context.RatingCategories.Update(ratingCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ratingCategory = await GetByIdAsync(id);
            if (ratingCategory != null)
            {
                _context.RatingCategories.Remove(ratingCategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> CalculateAverageRatingByProductIdAsync(int productId)
        {
            var ratingCategories = await GetAllByProductIdAsync(productId);

            if (!ratingCategories.Any())
                return 0;

            double totalRating = 0;
            int totalCount = 0;

            foreach (var category in ratingCategories)
            {
                int starValue = int.Parse(category.CategoryName.Split(' ')[0]);
                totalRating += starValue * (category.TotalRatings ?? 0);
                totalCount += category.TotalRatings ?? 0;
            }

            double averageRating = totalCount > 0 ? totalRating / totalCount : 0;
            return Math.Round(averageRating, 1);
        }
    }
}