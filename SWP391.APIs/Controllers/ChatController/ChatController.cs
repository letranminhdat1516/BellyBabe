using Microsoft.AspNetCore.Mvc;
using SWP391.BLL.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using SWP391.DAL.Model.Chat;

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
        return Ok(new { message = $"Customer chose option: {model.OptionValue}" });
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _chatService.GetMessagesAsync();
        return Ok(messages);
    }

    [HttpGet("customer-options")]
    public async Task<IActionResult> GetCustomerOptions()
    {
        var options = await _chatService.GetCustomerOptionsAsync();
        return Ok(options);
    }
}
