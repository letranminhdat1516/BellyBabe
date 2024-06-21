using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.PriceUpdateRepository
{
    public class PriceUpdateRepository
    {
        private readonly Swp391Context _context;

        public PriceUpdateRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddPriceUpdate(decimal? price, DateTime? dateApplied, int? productId)
        {
            var newPriceUpdate = new PriceUpdate
            {
                Price = price,
                DateApplied = dateApplied,
                ProductId = productId
            };

            _context.PriceUpdates.Add(newPriceUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePriceUpdate(int priceId)
        {
            var priceUpdate = await _context.PriceUpdates.FindAsync(priceId);
            if (priceUpdate != null)
            {
                _context.PriceUpdates.Remove(priceUpdate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePriceUpdate(int priceId, Dictionary<string, object> updates)
        {
            var priceUpdate = await _context.PriceUpdates.FindAsync(priceId);

            if (priceUpdate != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "price":
                            priceUpdate.Price = (decimal?)update.Value;
                            break;
                        case "dateApplied":
                            priceUpdate.DateApplied = (DateTime?)update.Value;
                            break;
                        case "productId":
                            priceUpdate.ProductId = (int?)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PriceUpdate>> GetAllPriceUpdates()
        {
            return await _context.PriceUpdates
                .Include(pu => pu.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PriceUpdate?> GetPriceUpdateById(int priceId)
        {
            return await _context.PriceUpdates
                .Include(pu => pu.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(pu => pu.PriceId == priceId);
        }

        public async Task<List<PriceUpdate>> GetPriceUpdatesByProductId(int productId)
        {
            return await _context.PriceUpdates
                .Where(pu => pu.ProductId == productId)
                .Include(pu => pu.Product)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
