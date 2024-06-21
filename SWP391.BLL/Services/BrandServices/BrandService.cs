﻿using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.BrandRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class BrandService
    {
        private readonly BrandRepository _brandRepository;

        public BrandService(BrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task AddBrand(string brandName, string? description)
        {
            await _brandRepository.AddBrand(brandName, description);
        }

        public async Task DeleteBrand(int brandId)
        {
            await _brandRepository.DeleteBrand(brandId);
        }

        public async Task UpdateBrand(int brandId, Dictionary<string, object> updates)
        {
            await _brandRepository.UpdateBrand(brandId, updates);
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrands();
        }

        public async Task<Brand?> GetBrandById(int brandId)
        {
            return await _brandRepository.GetBrandById(brandId);
        }
    }
}
