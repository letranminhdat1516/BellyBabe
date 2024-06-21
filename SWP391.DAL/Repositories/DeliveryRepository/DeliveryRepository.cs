using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.DeliveryRepository
{
    public class DeliveryRepository
    {
        private readonly Swp391Context _context;

        public DeliveryRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddDelivery(string deliveryName, string deliveryMethod, decimal deliveryFee)
        {
            var newDelivery = new Delivery
            {
                DeliveryName = deliveryName,
                DeliveryMethod = deliveryMethod,
                DeliveryFee = deliveryFee
            };

            _context.Deliveries.Add(newDelivery);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDelivery(int deliveryId)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDelivery(int deliveryId, Dictionary<string, object> updates)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);

            if (delivery != null)
            {
                foreach (var update in updates)
                {
                    switch (update.Key)
                    {
                        case "deliveryName":
                            delivery.DeliveryName = (string)update.Value;
                            break;
                        case "deliveryMethod":
                            delivery.DeliveryMethod = (string)update.Value;
                            break;
                        case "deliveryFee":
                            delivery.DeliveryFee = (decimal)update.Value;
                            break;
                        default:
                            throw new ArgumentException($"Invalid property name: {update.Key}", nameof(updates));
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Delivery>> GetAllDeliveries()
        {
            return await _context.Deliveries
                .Include(d => d.Orders)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Delivery?> GetDeliveryById(int deliveryId)
        {
            return await _context.Deliveries
                .Include(d => d.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
        }
    }
}
