using Copilot.Api.Extentions;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

[Controller]
public class InstructionFileController : ControllerBase
{
    private readonly IInstructionFileService _instructionFileService;

    public InstructionFileController(IInstructionFileService instructionFileService)
    {
        _instructionFileService = instructionFileService;
    }
    
    [HttpPost("api/files/")]
    public async Task<IActionResult> Create([FromForm] AddFileAsKnowledgeRequestModel requestModel)
    {
        await using var stream = requestModel.Content.OpenReadStream();
        
        var model = new AddInstructionFile
        {
            FileName = requestModel.Content.Name,
            Content = stream,
            BotId = requestModel.BotId,
            UserId = User.GetId()
        };

        return Ok(await _instructionFileService.CreateInstructionFile(model));
    }
    
    [HttpGet("api/files/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var instructionFileResponse = await _instructionFileService.GetInstructionFileByBotId(User.GetId(), id);
        return Ok(instructionFileResponse);
    }
    
    [HttpDelete("api/files/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _instructionFileService.DeleteInstructionFile(User.GetId(), id);
        return NoContent();
    }
}