using System;
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
            return await _context.Set<RatingCategory>().FindAsync(id);
        }

        public async Task<IEnumerable<RatingCategory>> GetAllAsync()
        {
            return await _context.Set<RatingCategory>().ToListAsync();
        }

        public async Task<IEnumerable<RatingCategory>> GetAllByProductIdAsync(int productId)
        {
            return await _context.Set<RatingCategory>()
                .Where(rc => rc.ProductId == productId)
                .ToListAsync();
        }

        public async Task CreateAsync(RatingCategory ratingCategory)
        {
            await _context.Set<RatingCategory>().AddAsync(ratingCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RatingCategory ratingCategory)
        {
            _context.Set<RatingCategory>().Update(ratingCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ratingCategory = await GetByIdAsync(id);
            if (ratingCategory != null)
            {
                _context.Set<RatingCategory>().Remove(ratingCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
