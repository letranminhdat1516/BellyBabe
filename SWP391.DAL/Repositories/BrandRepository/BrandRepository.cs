using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Text.Json;
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

        public async Task UpdateBrand(int brandId, string? brandName, string? description, string? imageBrand)
        {
            var brand = await _context.Brands.FindAsync(brandId);

            if (brand == null)
            {
                throw new ArgumentException("Thương hiệu không tồn tại.");
            }

            if (brandName != null)
            {
                if (string.IsNullOrWhiteSpace(brandName) || brandName.Length > 100)
                {
                    throw new ArgumentException("Tên thương hiệu không được để trống và phải dưới 100 ký tự.");
                }

            }

            brand.BrandName = brandName ?? brand.BrandName;
            brand.Description = description ?? brand.Description;
            brand.ImageBrand = imageBrand ?? brand.ImageBrand;

            await _context.SaveChangesAsync();
        }


        public async Task<List<Brand>> GetAllBrands()
        {
            return await _context.Brands
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Brand?> GetBrandById(int brandId)
        {
            return await _context.Brands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BrandId == brandId);
        }
    }
}
