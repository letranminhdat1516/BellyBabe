﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories;

namespace SWP391.BLL.Services.CartServices
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;

        public CartService(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<(List<OrderDetail>, string)> GetCartDetailsAsync(int userId)
        {
            try
            {
                var orderDetails = await _cartRepository.GetCartDetailsAsync(userId);
                return (orderDetails, "Đã lấy thông tin giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                return (null, $"Lấy chi tiết giỏ hàng thất bại: {ex.Message}");
            }
        }

        public async Task<string> AddProductToCartAsync(int userId, int productId, int quantity)
        {
            try
            {
                var user = await _cartRepository.GetUserAsync(userId);
                if (user == null)
                {
                    return "ID người dùng không hợp lệ.";
                }

                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                if (product.IsSelling != true)
                {
                    return "Sản phẩm hiện không có sẵn để mua.";
                }

                if (quantity <= 0)
                {
                    return "Số lượng sản phẩm phải lớn hơn 0.";
                }

                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);

                if (orderDetail == null)
                {
                    orderDetail = new OrderDetail
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        Price = (int)(product.NewPrice * quantity)
                    };
                    await _cartRepository.AddOrderDetailAsync(orderDetail);
                }
                else
                {
                    orderDetail.Quantity += quantity;
                    orderDetail.Price = (int)(product.NewPrice * orderDetail.Quantity);
                    await _cartRepository.UpdateOrderDetailAsync(orderDetail);
                }

                return "Đã thêm sản phẩm vào giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                return $"Thêm sản phẩm vào giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> PurchaseNowAsync(int userId, int productId, int quantity)
        {
            try
            {
                var user = await _cartRepository.GetUserAsync(userId);
                if (user == null)
                {
                    return "ID người dùng không hợp lệ.";
                }

                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                if (product.IsSelling != true)
                {
                    return "Sản phẩm hiện không có sẵn để mua.";
                }

                if (quantity <= 0)
                {
                    return "Số lượng sản phẩm phải lớn hơn 0.";
                }

                var orderDetail = new OrderDetail
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = (int)(product.NewPrice * quantity)
                };

                await _cartRepository.AddOrderDetailAsync(orderDetail);

                return "Đã mua ngay sản phẩm thành công.";
            }
            catch (Exception ex)
            {
                return $"Mua ngay sản phẩm thất bại: {ex.Message}";
            }
        }

        public async Task<string> IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            try
            {
                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);
                if (orderDetail == null)
                {
                    return "Không tìm thấy sản phẩm trong giỏ hàng.";
                }

                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                if (product.IsSelling != true)
                {
                    return "Sản phẩm hiện không có sẵn để mua.";
                }

                if (product.Quantity < orderDetail.Quantity + quantityToAdd)
                {
                    return "Không đủ số lượng sản phẩm để thêm vào giỏ hàng.";
                }

                orderDetail.Quantity += quantityToAdd;
                orderDetail.Price = (int)(product.NewPrice * orderDetail.Quantity);

                await _cartRepository.UpdateOrderDetailAsync(orderDetail);

                return "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            try
            {
                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);
                if (orderDetail == null)
                {
                    return "Không tìm thấy sản phẩm trong giỏ hàng.";
                }

                var product = await _cartRepository.GetProductAsync(productId);
                if (product == null)
                {
                    return "ID sản phẩm không hợp lệ.";
                }

                if (product.IsSelling != true)
                {
                    return "Sản phẩm hiện không có sẵn để mua.";
                }

                orderDetail.Quantity -= quantityToSubtract;
                if (orderDetail.Quantity <= 0)
                {
                    await _cartRepository.RemoveOrderDetailAsync(orderDetail);
                    return "Sản phẩm đã được xóa khỏi giỏ hàng do số lượng bằng 0.";
                }
                else
                {
                    orderDetail.Price = (int)(product.NewPrice * orderDetail.Quantity);
                    await _cartRepository.UpdateOrderDetailAsync(orderDetail);
                }

                return "Số lượng sản phẩm trong giỏ hàng đã được cập nhật thành công.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật số lượng sản phẩm trong giỏ hàng thất bại: {ex.Message}";
            }
        }

        public async Task<string> DeleteProductFromCartAsync(int userId, int productId)
        {
            try
            {
                var orderDetail = await _cartRepository.GetOrderDetailAsync(userId, productId);
                if (orderDetail == null)
                {
                    return "Không tìm thấy sản phẩm trong giỏ hàng.";
                }

                await _cartRepository.RemoveOrderDetailAsync(orderDetail);

                return "Sản phẩm đã được xóa khỏi giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                return $"Xóa sản phẩm khỏi giỏ hàng thất bại: {ex.Message}";
            }
        }
    }
}