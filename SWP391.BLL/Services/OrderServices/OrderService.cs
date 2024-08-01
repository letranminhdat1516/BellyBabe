using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Order;
using SWP391.DAL.Repositories.OrderRepository;
using Microsoft.EntityFrameworkCore;
namespace SWP391.BLL.Services.OrderServices
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly VoucherService _voucherService;

        public OrderService(OrderRepository orderRepository, VoucherService voucherService)
        {
            _orderRepository = orderRepository;
            _voucherService = voucherService;
        }
        public async Task<Order> PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, string? note, bool? usePoints = null, string? voucherCode = null)
        {
         
            var order = await _orderRepository.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, note, usePoints, voucherCode);
            return order;
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
        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderDTOs = orders.Select(o => new OrderModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                Note = o.Note,
                VoucherId = o.VoucherId,
                TotalPrice = o.TotalPrice,
                OrderDate = o.OrderDate,
                RecipientName = o.RecipientName,
                RecipientPhone = o.RecipientPhone,
                RecipientAddress = o.RecipientAddress,
                PointsUsed = o.PointsUsed,
                StatusId = o.StatusId
            }).ToList();

            return orderDTOs;
        }


        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _orderRepository.GetTotalOrdersCountAsync();
        }

    }
}
