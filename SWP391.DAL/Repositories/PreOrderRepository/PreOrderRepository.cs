﻿using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.PreOrderRepository
{
    public class PreOrderRepository
    {
        private readonly Swp391Context _context;

        public PreOrderRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<PreOrder> AddPreOrderAsync(PreOrder preOrder)
        {
            await _context.PreOrders.AddAsync(preOrder);
            await _context.SaveChangesAsync();
            return preOrder;
        }

        public async Task<IEnumerable<PreOrder>> GetPreOrdersByUserIdAsync(int userId)
        {
            return await _context.PreOrders
                .Include(p => p.Product)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }

}
