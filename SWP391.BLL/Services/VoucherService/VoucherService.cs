using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Model.Voucher;

namespace SWP391.BLL.Services
{
    public class VoucherService
    {
        private readonly Swp391Context _context;

        public VoucherService(Swp391Context context)
        {
            _context = context;
        }

        public async Task<List<Voucher>> GetVouchersAsync()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(int id)
        {
            return await _context.Vouchers.FindAsync(id);
        }

        public async Task<Voucher> AddVoucherAsync(VoucherDTO voucherDTO)
        {
            var voucher = new Voucher
            {
                VoucherCode = voucherDTO.VoucherCode,
                VoucherName = voucherDTO.VoucherName,
                Quantity = voucherDTO.Quantity,
                ExpiredDate = voucherDTO.ExpiredDate,
                Price = voucherDTO.Price,
                MinimumBillAmount = voucherDTO.MinimumBillAmount
            };

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<Voucher> UpdateVoucherAsync(int id, VoucherDTO voucherDTO)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return null;
            }

            voucher.VoucherCode = voucherDTO.VoucherCode;
            voucher.VoucherName = voucherDTO.VoucherName;
            voucher.Quantity = voucherDTO.Quantity;
            voucher.ExpiredDate = voucherDTO.ExpiredDate;
            voucher.Price = voucherDTO.Price;
            voucher.MinimumBillAmount = voucherDTO.MinimumBillAmount;

            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return false;
            }

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateVoucherAsync(string voucherCode)
        {
            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherCode == voucherCode && v.Quantity > 0 && v.ExpiredDate > DateTime.Now);

            if (voucher != null)
            {
                voucher.Quantity -= 1;
                _context.Vouchers.Update(voucher);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

}

