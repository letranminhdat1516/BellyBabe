using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.PreOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.PreOrderService
{
    public class PreOrderService
    {
        private readonly PreOrderRepository _preOrderRepository;

        public PreOrderService(PreOrderRepository preOrderRepository)
        {
            _preOrderRepository = preOrderRepository;
        }

        public async Task<PreOrder> CreatePreOrderAsync(int userId, int productId)
        {
            var preOrder = new PreOrder
            {
                UserId = userId,
                ProductId = productId,
                PreOrderDate = DateTime.UtcNow,
                NotificationSent = false
            };

            return await _preOrderRepository.AddPreOrderAsync(preOrder);
        }

        public async Task<IEnumerable<PreOrder>> GetPreOrdersByUserIdAsync(int userId)
        {
            return await _preOrderRepository.GetPreOrdersByUserIdAsync(userId);
        }
    }

}
