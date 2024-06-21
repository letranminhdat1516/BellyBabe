using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.ManufacturerRepository
{
    public class ManufacturerRepository
    {
        private readonly Swp391Context _context;

        public ManufacturerRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddManufacturer(string manufacturerName)
        {
            var newManufacturer = new Manufacturer
            {
                ManufacturerName = manufacturerName
            };

            _context.Manufacturers.Add(newManufacturer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteManufacturer(int manufacturerId)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(manufacturerId);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateManufacturer(int manufacturerId, Dictionary<string, object> updates)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(manufacturerId);

            if (manufacturer != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "manufacturerName":
                            manufacturer.ManufacturerName = (string)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Manufacturer>> GetAllManufacturers()
        {
            return await _context.Manufacturers
                .Include(m => m.Products)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Manufacturer?> GetManufacturerById(int manufacturerId)
        {
            return await _context.Manufacturers
                .Include(m => m.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ManufacturerId == manufacturerId);
        }
    }
}
