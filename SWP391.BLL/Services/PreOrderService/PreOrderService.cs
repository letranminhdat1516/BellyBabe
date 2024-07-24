using SWP391.DAL.Entities;

using SWP391.DAL.Repositories.PreOrderRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Model.PreOrder;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Repositories.UserRepository;
using SWP391.DAL.Repositories.ProductRepository;
using SWP391.DAL.Repositories.Contract;

namespace SWP391.BLL.Services.PreOrderService
{
    public class PreOrderService
    {
        private readonly PreOrderRepository _preOrderRepository;
        private readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ProductRepository _productRepository;

        public PreOrderService(PreOrderRepository preOrderRepository, EmailService emailService, IUserRepository userRepository,ProductRepository productRepository)
        {
            _preOrderRepository = preOrderRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<PreOrderModel> CreatePreOrderAsync(CreatePreOrderModel model)
        {
            var preOrder = new PreOrder
            {
                UserId = model.UserId,
                ProductId = model.ProductId,
                PreOrderDate = DateTime.UtcNow,
                NotificationSent = false
            };

            var savedPreOrder = await _preOrderRepository.AddPreOrderAsync(preOrder);

            var emailSubject = "XÁC NHẬN ĐẶT TRƯỚC ĐƠN HÀNG";
            var emailBody = $"Thân gửi {model.Email},\n\nCảm ơn bạn đặt trước sản phẩm {model.ProductName}" +
                $". Chúng tôi sẽ thông báo cho bạn khi đơn hàng của bạn được xử lý.\n\n" +
                $"Trân trọng,\nBelly&Babe";
            await _emailService.SendEmailAsync(model.Email, emailSubject, emailBody);

            return new PreOrderModel
            {
                PreOrderId = savedPreOrder.PreOrderId,
                UserId = savedPreOrder.UserId,
                ProductId = savedPreOrder.ProductId,
                ProductName = model.ProductName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PreOrderDate = savedPreOrder.PreOrderDate,
                NotificationSent = savedPreOrder.NotificationSent
            };
        }
        public async Task<PreOrderModel> CreateOrUpdatePreOrderAsync(CreatePreOrderModel model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.UserId);
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);

            if (user == null || product == null)
            {
                throw new Exception("User or Product not found");
            }

            // Chuẩn hóa số điện thoại
            string normalizedPhoneNumber = NormalizePhoneNumber(model.PhoneNumber);
            if (user.PhoneNumber != normalizedPhoneNumber)
            {
                user.PhoneNumber = normalizedPhoneNumber;
                await _userRepository.UpdateUserAsync(user); // Cập nhật số điện thoại nếu khác
            }

            // Cập nhật email nếu khác
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                await _userRepository.UpdateUserAsync(user); // Cập nhật email nếu khác
            }

            var preOrder = new PreOrder
            {
                UserId = model.UserId,
                ProductId = model.ProductId,
                PreOrderDate = DateTime.UtcNow,
                NotificationSent = false
            };

            var savedPreOrder = await _preOrderRepository.AddOrUpdatePreOrderAsync(preOrder);

            var emailSubject = "XÁC NHẬN ĐẶT TRƯỚC ĐƠN HÀNG";
            var emailBody = $"Thân gửi {user.Email},\n\nCảm ơn bạn đặt trước sản phẩm {product.ProductName}" +
                $". Chúng tôi sẽ thông báo cho bạn khi đơn hàng của bạn được xử lý.\n\n" +
                $"Trân trọng,\nBelly&Babe";
            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return new PreOrderModel
            {
                PreOrderId = savedPreOrder.PreOrderId,
                UserId = savedPreOrder.UserId,
                ProductId = savedPreOrder.ProductId,
                ProductName = product.ProductName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                PreOrderDate = savedPreOrder.PreOrderDate,
                NotificationSent = savedPreOrder.NotificationSent
            };
        }
        private string NormalizePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("0"))
            {
                return "84" + phoneNumber.Substring(1);
            }
            return phoneNumber;
        }



        public async Task NotifyCustomerAsync(NotifyCustomerModel model)
        {
            var emailSubject = "THÔNG BÁO SẢN PHẨM BẠN ĐẶT TRƯỚC TỪ CỬA HÀNG BELLY&BABE ĐÃ CÓ";
            var emailBody = $"Thân gửi,\n\nSản phẩm {model.ProductName} bạn đã đặt trước đã có." +
                $" Vui lòng ghé thăm website cửa hàng của chúng tôi để hoàn tất việc mua hàng của bạn." +
                $"\n\nTrân trọng,\nBelly&Babe";
            await _emailService.SendEmailAsync(model.Email, emailSubject, emailBody);

            await _preOrderRepository.UpdateNotificationSentAsync(model.PreOrderId);
        }

        public async Task<IEnumerable<PreOrderModel>> GetPreOrdersByUserIdAsync(int userId)
        {
            var preOrders = await _preOrderRepository.GetPreOrdersByUserIdAsync(userId);
            var preOrderModels = new List<PreOrderModel>();

            foreach (var preOrder in preOrders)
            {
                preOrderModels.Add(new PreOrderModel
                {
                    PreOrderId = preOrder.PreOrderId,
                    UserId = preOrder.UserId,
                    ProductId = preOrder.ProductId,
                    ProductName = preOrder.Product.ProductName,
                    PhoneNumber = preOrder.User.PhoneNumber,
                    Email = preOrder.User.Email, 
                    PreOrderDate = preOrder.PreOrderDate,
                    NotificationSent = preOrder.NotificationSent
                });
            }

            return preOrderModels;
        }

        public async Task<List<PreOrderModel>> GetAllPreOrdersAsync()
        {
            var preOrders = await _preOrderRepository.GetAllPreOrdersAsync();
            var preOrderModels = new List<PreOrderModel>();

            foreach (var preOrder in preOrders)
            {
                if (preOrder != null)
                {
                    var preOrderModel = new PreOrderModel
                    {
                        PreOrderId = preOrder.PreOrderId,
                        UserId = preOrder.UserId,
                        ProductId = preOrder.ProductId,
                        ProductName = preOrder.Product?.ProductName,
                        PhoneNumber = preOrder.User?.PhoneNumber,
                        Email = preOrder.User?.Email,
                        PreOrderDate = preOrder.PreOrderDate,
                        NotificationSent = preOrder.NotificationSent
                    };

                    preOrderModels.Add(preOrderModel);
                }
            }

            return preOrderModels;
        }
        public async Task<bool> DeletePreOrderAsync(int preOrderId)
        {
            return await _preOrderRepository.DeletePreOrderAsync(preOrderId);
        }

    }
}
