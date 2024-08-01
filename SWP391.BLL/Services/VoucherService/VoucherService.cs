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
        private static Queue<Func<Task>> _voucherQueue = new Queue<Func<Task>>();
        private static object _queueLock = new object();

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
        //
        public async Task<bool> ValidateVoucherAsync(string voucherCode)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.VoucherName == voucherCode && v.Quantity > 0 && v.ExpiredDate > DateTime.Now);

                    if (voucher != null)
                    {
                        voucher.Quantity -= 1;
                        _context.Vouchers.Update(voucher);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }

                    return false;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        public async Task<decimal> ApplyVoucherAsync(string voucherCode, decimal totalBillAmount)
        {
            var isValid = await ValidateVoucherAsync(voucherCode);

            if (!isValid)
            {
                throw new Exception("Invalid or expired voucher code.");
            }

            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.VoucherCode == voucherCode);

            if (voucher != null)
            {
                if (totalBillAmount >= voucher.MinimumBillAmount)
                {
                    totalBillAmount -= (voucher.Price ?? 0); 
                                                         
                }
            }

            return totalBillAmount;
        }

        public async Task<bool> ValidateVoucherWithQueueAsync(string voucherCode)
        {
            var tcs = new TaskCompletionSource<bool>();

            lock (_queueLock)
            {
                _voucherQueue.Enqueue(async () =>
                {
                    var result = await ValidateVoucherAsync(voucherCode);
                    tcs.SetResult(result);
                });

                if (_voucherQueue.Count == 1)
                {
                    ProcessQueue();
                }
            }

            return await tcs.Task;
        }
        private async void ProcessQueue()
        {
            while (true)
            {
                Func<Task> action;

                lock (_queueLock)
                {
                    if (_voucherQueue.Count == 0) return;
                    action = _voucherQueue.Dequeue();
                }

                await action.Invoke();
            }
        }
    }

}

