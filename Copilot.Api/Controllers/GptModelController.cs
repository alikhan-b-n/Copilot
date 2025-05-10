using Copilot.ChatBotManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

[Controller]

public class GptModelController : ControllerBase
{
    private readonly IGptModelService _gptModelService;

    public GptModelController(IGptModelService gptModelService)
    {
        _gptModelService = gptModelService;
    }
    
    [HttpGet("api/gptmodels")]
    public async Task<IActionResult> GetAll()
    {
        var gptModels = await _gptModelService.GetAllGptModels();

        return Ok(gptModels);
    }
}
