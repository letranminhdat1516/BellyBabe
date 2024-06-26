using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.BrandRepository
{
    public class BrandRepository
    {
        private readonly Swp391Context _context;

        public BrandRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddBrand(string brandName, string? description, string? imageBrand)
        {
            var newBrand = new Brand
            {
                BrandName = brandName,
                Description = description,
                ImageBrand = imageBrand
            };

            _context.Brands.Add(newBrand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBrand(int brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBrand(int brandId, Dictionary<string, object> updates)
        {
            var brand = await _context.Brands.FindAsync(brandId);

            if (brand != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "brandName":
                            brand.BrandName = (string)update.Value;
                            break;
                        case "description":
                            brand.Description = (string)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _context.Brands
                .Include(b => b.Products)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Brand?> GetBrandById(int brandId)
        {
            return await _context.Brands
                .Include(b => b.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BrandId == brandId);
        }
    }
}
