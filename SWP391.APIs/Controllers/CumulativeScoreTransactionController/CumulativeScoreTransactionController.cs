using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services.CumulativeScoreTransactionServices;
using SWP391.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CumulativeScoreTransactionController : ControllerBase
    {
        private readonly CumulativeScoreTransactionService _cumulativeScoreTransactionService;

        public CumulativeScoreTransactionController(CumulativeScoreTransactionService cumulativeScoreTransactionService)
        {
            _cumulativeScoreTransactionService = cumulativeScoreTransactionService;
        }

        [HttpPost("{userId}/addTransaction")]
        public async Task<IActionResult> AddTransaction(int userId, int? orderId, int scoreChange, string transactionType)
        {
            try
            {
                await _cumulativeScoreTransactionService.AddTransactionAsync(userId, orderId, scoreChange, transactionType);
                return Ok($"Giao dịch thành công: {transactionType} {scoreChange} điểm cho người dùng {userId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi thực hiện giao dịch: {ex.Message}");
            }
        }

        [HttpGet("{userId}/transactions")]
        public async Task<IActionResult> GetUserTransactions(int userId)
        {
            try
            {
                var transactions = await _cumulativeScoreTransactionService.GetUserTransactionsAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy giao dịch của người dùng: {ex.Message}");
            }
        }
    }
}
