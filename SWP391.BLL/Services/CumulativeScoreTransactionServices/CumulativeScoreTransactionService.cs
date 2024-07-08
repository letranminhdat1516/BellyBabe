using SWP391.DAL.Repositories.CumulativeScoreTransactionRepository;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.CumulativeScoreTransactionServices
{
    public class CumulativeScoreTransactionService
    {
        private readonly CumulativeScoreTransactionRepository _cumulativeScoreTransactionRepository;

        public CumulativeScoreTransactionService(CumulativeScoreTransactionRepository cumulativeScoreTransactionRepository)
        {
            _cumulativeScoreTransactionRepository = cumulativeScoreTransactionRepository;
        }

        public async Task AddTransactionAsync(int userId, int? orderId, int scoreChange, string transactionType)
        {
            await _cumulativeScoreTransactionRepository.AddTransactionAsync(userId, orderId, scoreChange, transactionType);
        }

        public async Task<List<CumulativeScoreTransaction>> GetUserTransactionsAsync(int userId)
        {
            return await _cumulativeScoreTransactionRepository.GetUserTransactionsAsync(userId);
        }
    }
}
