using SWP391.DAL.Entities;

using SWP391.DAL.Repositories.PreOrderRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Model.PreOrder;

namespace SWP391.BLL.Services.PreOrderService
{
    public class PreOrderService
    {
        private readonly PreOrderRepository _preOrderRepository;
        private readonly EmailService _emailService;

        public PreOrderService(PreOrderRepository preOrderRepository, EmailService emailService)
        {
            _preOrderRepository = preOrderRepository;
            _emailService = emailService;
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

    }
}
