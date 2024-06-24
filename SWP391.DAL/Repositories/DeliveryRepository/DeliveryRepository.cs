using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddDelivery(string deliveryName, int? deliveryFee)
        {
            if (string.IsNullOrEmpty(deliveryName))
            {
                throw new ArgumentException("Tên phương thức giao hàng không được để trống.");
            }

            if (deliveryFee.HasValue && deliveryFee < 0)
            {
                throw new ArgumentException("Phí giao hàng phải lớn hơn hoặc bằng 0.");
            }

            var newDelivery = new Delivery
            {
                DeliveryName = deliveryName,
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

        public async Task UpdateDelivery(int deliveryId, string deliveryName, int? deliveryFee)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);

            if (delivery != null)
            {
                if (!string.IsNullOrEmpty(deliveryName))
                {
                    delivery.DeliveryName = deliveryName;
                }
                else
                {
                    throw new ArgumentException("Tên phương thức giao hàng không được để trống.");
                }

                if (deliveryFee.HasValue && deliveryFee >= 0)
                {
                    delivery.DeliveryFee = deliveryFee;
                }
                else
                {
                    throw new ArgumentException("Phí giao hàng phải lớn hơn hoặc bằng 0.");
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Không tìm thấy phương thức giao hàng.");
            }
        }

        public async Task<List<Delivery>> GetAllDeliveries()
        {
            return await _context.Deliveries
                .Include(d => d.Orders)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Delivery> GetDeliveryById(int deliveryId)
        {
            var delivery = await _context.Deliveries
                .Include(d => d.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);

            if (delivery == null)
            {
                throw new KeyNotFoundException("Không tìm thấy phương thức giao hàng.");
            }

            return delivery;
        }
    }
}
