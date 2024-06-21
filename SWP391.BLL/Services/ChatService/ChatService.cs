using SWP391.DAL.Entities;
using SWP391.DAL.Entities.Chat;
using SWP391.DAL.Swp391DbContext;
using System.Threading.Tasks;

public class ChatService
{
    private readonly Swp391Context _context;

    public ChatService(Swp391Context context)
    {
        _context = context;
    }

    public async Task SaveMessageAsync(ChatMessageModel message)
    {
        // Tạo bản ghi mới trong bảng Message
        var newMessage = new Message
        {
            UserName = message.FromUser,
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
            FromUserName = message.FromUser,
            ToUserName = message.ToUser,
            MessageId = messageId,
            IsView = false,
            DateCreated = DateTime.Now
        };

        // Tạo bản ghi mới trong bảng MessageOutboxUser
        var outboxMessage = new MessageOutboxUser
        {
            FromUserName = message.FromUser,
            ToUserName = message.ToUser,
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
        // Lưu thông tin lựa chọn của khách hàng vào bảng tùy chọn hoặc một bảng liên quan
        var customerOption = new CustomerOption
        {
            UserName = model.UserName,
            Option = model.Option,
            DateSelected = DateTime.Now
        };

        _context.CustomerOptions.Add(customerOption);
        await _context.SaveChangesAsync();
    }
}
