using Copilot.Api.Extentions;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

[Controller]
[Authorize]
public class ChatBotController : ControllerBase
{
    private readonly ICopilotChatBotService _chatBotService;

    public ChatBotController(ICopilotChatBotService chatBotService)
    {
        _chatBotService = chatBotService;
    }

    [HttpGet("api/chatbots")]
    public async Task<IActionResult> GetAll()
    {
        var chatBots = await _chatBotService.GetAllChatBots(User.GetId());

        return Ok(chatBots);
    }

    [HttpGet("api/chatbots/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var chatBot = await _chatBotService.GetChatBotById(User.GetId(), id);

        return Ok(chatBot);
    }

    [HttpPost("api/chatbots/")]
    public async Task<IActionResult> Create([FromBody] CopilotChatBotRequest chatBotRequest)
    {
        var createdChatBot = await _chatBotService.CreateChatBot(User.GetId(), chatBotRequest);
        
        return Ok(createdChatBot);
    }

    [HttpPut("api/chatbots/{id}")]
    public async Task<IActionResult> Update(Guid id, 
        [FromBody] CopilotChatBotUpdateRequest chatBotRequest)
    {
        var updatedChatBot = await _chatBotService.UpdateChatBot(id, User.GetId(), chatBotRequest);

        return Ok(updatedChatBot);  
    }

    [HttpDelete("api/chatbots/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _chatBotService.DeleteChatBot(id, User.GetId());

        return NoContent();
    }
}