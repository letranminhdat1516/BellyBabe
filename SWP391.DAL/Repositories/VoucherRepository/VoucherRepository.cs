using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.VoucherRepository
{
    public class VoucherRepository
    {
        private readonly Swp391Context _context;

        public VoucherRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddVoucherAsync(Voucher voucher)
        {
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVoucherAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVoucherAsync(int voucherId)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Voucher>> GetVouchersAsync()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(int voucherId)
        {
            return await _context.Vouchers.FindAsync(voucherId);
        }
    }
}
