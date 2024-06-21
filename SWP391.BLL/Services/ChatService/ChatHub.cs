using Microsoft.AspNetCore.SignalR;
using SWP391.DAL.Entities.Chat;
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
        await _chatService.SaveMessageAsync(message);
        await Clients.User(message.ToUser).SendAsync("ReceiveMessage", message);
    }
}
