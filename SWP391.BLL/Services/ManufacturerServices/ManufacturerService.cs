using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.ManufacturerRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services
{
    public class ManufacturerService
    {
        private readonly ManufacturerRepository _manufacturerRepository;

        public ManufacturerService(ManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task AddManufacturer(string manufacturerName)
        {
            await _manufacturerRepository.AddManufacturer(manufacturerName);
        }

        public async Task DeleteManufacturer(int manufacturerId)
        {
            await _manufacturerRepository.DeleteManufacturer(manufacturerId);
        }

        public async Task UpdateManufacturer(int manufacturerId, Dictionary<string, object> updates)
        {
            await _manufacturerRepository.UpdateManufacturer(manufacturerId, updates);
        }

        public async Task<List<Manufacturer>> GetAllManufacturers()
        {
            return await _manufacturerRepository.GetAllManufacturers();
        }

        public async Task<Manufacturer?> GetManufacturerById(int manufacturerId)
        {
            return await _manufacturerRepository.GetManufacturerById(manufacturerId);
        }
    }
}
