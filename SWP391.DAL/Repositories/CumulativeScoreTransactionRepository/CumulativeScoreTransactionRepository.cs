using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Swp391DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Repositories.CumulativeScoreTransactionRepository
{
    public class CumulativeScoreTransactionRepository
    {
        private readonly Swp391Context _context;

        public CumulativeScoreTransactionRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(int userId, int? orderId, int scoreChange, string transactionType)
        {
            var transaction = new CumulativeScoreTransaction
            {
                UserId = userId,
                OrderId = orderId,
                ScoreChange = scoreChange,
                TransactionDate = DateTime.Now,
                TransactionType = transactionType
            };

            _context.CumulativeScoreTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CumulativeScoreTransaction>> GetUserTransactionsAsync(int userId)
        {
            return await _context.CumulativeScoreTransactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}
