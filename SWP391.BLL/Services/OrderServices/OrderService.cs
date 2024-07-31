using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Order;
using SWP391.DAL.Repositories.OrderRepository;

namespace SWP391.BLL.Services.OrderServices
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, string? note, bool? usePoints = null)
        {
            return await _orderRepository.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, note, usePoints);
        }

        public async Task UpdateOrderStatusAsync(int orderId, int statusId, string? note = null)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, statusId, note);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int userId, int statusId)
        {
            return await _orderRepository.GetOrdersByStatusAsync(userId, statusId);
        }

        public async Task CancelOrderAsync(int orderId, string reason)
        {
            await _orderRepository.CancelOrderAsync(orderId, reason);
        }

        public async Task AdminCancelOrderAsync(int orderId, string reason)
        {
            await _orderRepository.AdminCancelOrderAsync(orderId, reason);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<OrderStatusHistory> GetLatestOrderStatusAsync(int orderId)
        {
            return await _orderRepository.GetLatestOrderStatusAsync(orderId);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersAsync(userId);
        }
        public async Task<List<Order>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Số trang và kích thước trang phải lớn hơn 0.");
            }
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _orderRepository.GetTotalOrdersCountAsync();
        }

    }
}
