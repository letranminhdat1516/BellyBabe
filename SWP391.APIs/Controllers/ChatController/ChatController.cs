using Microsoft.AspNetCore.Mvc;
using SWP391.DAL.Entities.Chat;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessageModel message)
    {
        await _chatService.SaveMessageAsync(message);
        return Ok(new { message = "Message sent successfully" });
    }

    [HttpPost("customer-option")]
    public async Task<IActionResult> CustomerOption([FromBody] CustomerOptionModel model)
    {
        await _chatService.SaveCustomerOptionAsync(model);
        return Ok(new { message = $"Customer chose option: {model.Option}" });
    }
}
