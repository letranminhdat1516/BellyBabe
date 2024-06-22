using System;
using System.Collections.Generic;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.OrderRepository;
using System.Threading.Tasks;

namespace SWP391.BLL.Services.OrderServices
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string paymentMethod, string note)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("ID người dùng phải được cung cấp.");
            }

            await _orderRepository.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, deliveryId, paymentMethod, note);
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersAsync(userId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            if (orderId <= 0)
            {
                throw new ArgumentException("ID đơn hàng phải được cung cấp.");
            }

            await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int userId, string statusName)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("ID người dùng phải được cung cấp.");
            }

            return await _orderRepository.GetOrdersByStatusAsync(userId, statusName);
        }

        public async Task CancelOrderAsync(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentException("ID đơn hàng phải được cung cấp.");
            }

            await _orderRepository.CancelOrderAsync(orderId);
        }
    }
}
