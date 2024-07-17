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

        public async Task PlaceOrderAsync(int userId, string recipientName, string recipientPhone, string recipientAddress, int deliveryId, string? note, bool? usePoints = null)
        {
            try
            {
                await _orderRepository.PlaceOrderAsync(userId, recipientName, recipientPhone, recipientAddress, deliveryId, note, usePoints);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể đặt hàng: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Đặt hàng thất bại: {ex.Message}");
            }
        }

        public async Task<List<OrderModel>> GetOrdersAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("Vui lòng cung cấp ID người dùng.");
                }

                return await _orderRepository.GetOrdersAsync(userId);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể lấy đơn hàng: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy đơn hàng thất bại: {ex.Message}");
            }
        }


        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                if (orderId <= 0)
                {
                    throw new ArgumentException("Vui lòng cung cấp ID đơn hàng.");
                }

                await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể cập nhật trạng thái đơn hàng: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Cập nhật trạng thái đơn hàng thất bại: {ex.Message}");
            }
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int userId, string statusName)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("Vui lòng cung cấp ID người dùng.");
                }

                if (string.IsNullOrWhiteSpace(statusName))
                {
                    throw new ArgumentException("Vui lòng cung cấp tên trạng thái.");
                }

                return await _orderRepository.GetOrdersByStatusAsync(userId, statusName);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể lấy đơn hàng theo trạng thái: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy đơn hàng theo trạng thái thất bại: {ex.Message}");
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    throw new ArgumentException("Vui lòng cung cấp ID đơn hàng.");
                }

                await _orderRepository.CancelOrderAsync(orderId);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể hủy đơn hàng: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Hủy đơn hàng thất bại: {ex.Message}");
            }
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<OrderStatus> GetLatestOrderStatusAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    throw new ArgumentException("Vui lòng cung cấp ID đơn hàng.");
                }

                return await _orderRepository.GetLatestOrderStatusAsync(orderId);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể lấy trạng thái mới nhất của đơn hàng: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy trạng thái mới nhất của đơn hàng thất bại: {ex.Message}");
            }
        }

        public async Task<OrderModel> GetOrderByIdAsync(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    throw new ArgumentException("Vui lòng cung cấp ID đơn hàng.");
                }

                return await _orderRepository.GetOrderByIdAsync(orderId);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Không thể lấy đơn hàng: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lấy đơn hàng thất bại: {ex.Message}");
            }
        }
    }
}
