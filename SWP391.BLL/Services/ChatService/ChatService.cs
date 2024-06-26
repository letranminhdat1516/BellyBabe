using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using SWP391.DAL.Model.Chat;
using SWP391.DAL.Swp391DbContext;

public class ChatService
{
    private readonly Swp391Context _context;

    public ChatService(Swp391Context context)
    {
        _context = context;
    }

    public async Task SaveMessageAsync(ChatMessageModel message)
    {
        // Kiểm tra người dùng tồn tại trong hệ thống
        var fromUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == message.FromUserId);
        var toUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == message.ToUserId);

        if (fromUser == null && !message.IsAdmin)
        {
            throw new Exception("Người dùng gửi không tồn tại trong hệ thống.");
        }

        if (toUser == null && message.IsAdmin)
        {
            throw new Exception("Người dùng nhận không tồn tại trong hệ thống.");
        }

        // Tạo bản ghi mới trong bảng Message
        var newMessage = new Message
        {
            UserId = message.FromUserId,
            MessageContent = message.Message,
            Title = message.Title,
            DateCreated = DateTime.Now
        };

        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();

        // Lấy MessageId của tin nhắn vừa lưu
        int messageId = newMessage.MessageId;

        // Tạo bản ghi mới trong bảng MessageInboxUser
        var inboxMessage = new MessageInboxUser
        {
            FromUserId = message.FromUserId,
            ToUserId = message.ToUserId,
            MessageId = messageId,
            IsView = false,
            DateCreated = DateTime.Now
        };

        // Tạo bản ghi mới trong bảng MessageOutboxUser
        var outboxMessage = new MessageOutboxUser
        {
            FromUserId = message.FromUserId,
            ToUserId = message.ToUserId,
            MessageId = messageId,
            IsView = false,
            DateCreated = DateTime.Now
        };

        _context.MessageInboxUsers.Add(inboxMessage);
        _context.MessageOutboxUsers.Add(outboxMessage);
        await _context.SaveChangesAsync();
    }

    public async Task SaveCustomerOptionAsync(CustomerOptionModel model)
    {
        // Kiểm tra sự tồn tại của UserId
        if (model.UserId.HasValue)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == model.UserId.Value);
            if (!userExists)
            {
                throw new ArgumentException("User ID không tồn tại.");
            }
        }

        // Kiểm tra sự tồn tại của MessageId
        if (model.MessageId.HasValue)
        {
            var messageExists = await _context.Messages.AnyAsync(m => m.MessageId == model.MessageId.Value);
            if (!messageExists)
            {
                throw new ArgumentException("Message ID không tồn tại.");
            }
        }

        // Tạo đối tượng CustomerOption và gán giá trị từ model
        var customerOption = new CustomerOption
        {
            UserId = model.UserId ?? 0, // Chuyển đổi giá trị nullable int thành int, nếu null thì gán giá trị mặc định là 0
            MessageId = model.MessageId ?? 0,
            InboxId = model.InboxId ?? 0,
            OutboxId = model.OutboxId ?? 0,
            OptionValue = model.OptionValue,
        };

        // Thêm vào cơ sở dữ liệu và lưu lại
        _context.CustomerOptions.Add(customerOption);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessagesAsync()
    {
        return await _context.Messages.Include(m => m.User).ToListAsync();
    }

    public async Task<List<CustomerOption>> GetCustomerOptionsAsync()
    {
        return await _context.CustomerOptions.ToListAsync();
    }
}
