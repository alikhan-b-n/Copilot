using Copilot.Api.Extentions;
using Copilot.ChatBotManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

[Controller]
public class DialoguesController(IDialoguesService dialoguesService) : ControllerBase
{
    [HttpGet("api/chatbots/{botId:guid}/dialogues")]
    public async Task<IActionResult> GetAll(Guid botId, [FromQuery] bool showTest = false)
    {
        var userId = User.GetId();

        return Ok(await dialoguesService.GetAll(userId, botId, showTest));
    }
    
    [HttpPatch("api/chatbots/{botId:guid}/dialogues/{dialogueId:guid}")]
    public async Task<IActionResult> ChangeStatus(Guid botId, Guid dialogueId)
    {
        var userId = User.GetId();

        await dialoguesService.ChangeStatus(userId, botId, dialogueId);
        
        return Ok();
    }
    
    [HttpDelete("api/chatbots/{botId:guid}/dialogues/{dialogueId:guid}")]
    public async Task<IActionResult> Delete(Guid botId, Guid dialogueId)
    {
        var userId = User.GetId();

        await dialoguesService.Delete(userId, botId, dialogueId);
        
        return Ok();
    }
}