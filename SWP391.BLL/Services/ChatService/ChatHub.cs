using Microsoft.AspNetCore.SignalR;
using SWP391.DAL.Model.Chat;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessage(ChatMessageModel message)
    {
        // Lưu tin nhắn vào cơ sở dữ liệu
        await _chatService.SaveMessageAsync(message);

        // Gửi tin nhắn tới người nhận
        await Clients.User(message.ToUserId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public override async Task OnConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name ?? "bố mẹ";
        await Clients.Caller.SendAsync("ReceiveMessage", $"Xin chào {userName}");
        await base.OnConnectedAsync();
    }
}
