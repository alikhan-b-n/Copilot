using Copilot.Api.Extentions;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

public class AltegioCompanyController(IAltegioCompanyService altegioCompanyService) : ControllerBase
{
    [HttpGet("api/altegio/companies")]
    public async Task<IActionResult> GetAll()
    {
        var responses = await altegioCompanyService.GetAll(User.GetId());
        return Ok(responses);
    }

    [HttpGet("api/altegio/companies/{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await altegioCompanyService.GetById(id, User.GetId());

        return Ok(response);
    }

    [HttpPost("api/altegio/companies")]
    public async Task<IActionResult> Create([FromBody] CreateAltegioCompanyParameter parameter)
    {
        var result = await altegioCompanyService.Create(new AltegioCompanyResponse
        {
            CompanyId = parameter.CompanyId,
            CompanyName = parameter.CompanyName,
            UserId = User.GetId()
        });

        return Ok(result);
    }

    [HttpDelete("api/altegio/companies/{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
    {
        await altegioCompanyService.Delete(id, User.GetId());

        return NoContent();
    }
}