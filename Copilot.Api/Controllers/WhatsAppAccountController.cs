using Copilot.Api.Extentions;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using Copilot.WhatsApp.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

public class WhatsAppAccountController(
    IWhatsAppAccountService whatsAppAccountService,
    IFlowSellApi flowSellApi) : ControllerBase
{

    private readonly string webhookUrl = "http://91.147.92.159:5262/api/whatsapp/";

    [HttpGet("api/whatsapp/accounts")]
    public async Task<IActionResult> GetAll()
    {
        var instructionFileResponse = await whatsAppAccountService.GetAllWhatsAppAccounts(User.GetId());
        return Ok(instructionFileResponse);
    }

    [HttpGet("api/whatsapp/accounts/{id}")]
    public async Task<IActionResult> GetAccountSettings([FromRoute] Guid id)
    {
        var whatsAppAccountResponse = await whatsAppAccountService.GetById(User.GetId(), id);

        if (whatsAppAccountResponse != null)
        {
            return Ok(await flowSellApi.GetAccountSetting(whatsAppAccountResponse.IdInstance,
            whatsAppAccountResponse.ApiTokenInstance));
        }

        return BadRequest(new
        {
            message = "WhatsApp account is not found"
        });
    }

    [HttpPost("api/whatsapp/accounts/settings/{botId}")]
    public async Task<IActionResult> CreateAccount(Guid botId, [FromBody] CreateWhatsappAccountParameter parameter)
    {
        var instanceResponse = await flowSellApi.CreateInstance(webhookUrl + botId);

        var result = await whatsAppAccountService.Create(new WhatsAppAccountResponse
        {
            ApiTokenInstance = instanceResponse.ApiTokenInstance,
            IdInstance = instanceResponse.IdInstance,
            PhoneNumber = parameter.PhoneNumber,
            BotId = botId
        },
        User.GetId());

        return Ok(result);
    }

    [HttpDelete("api/whatsapp/accounts/settings/{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
    {
        var whatsAppAccountResponse = await whatsAppAccountService.Delete(id, User.GetId());

        return Ok(await flowSellApi.DeleteInstance(whatsAppAccountResponse.IdInstance));
    }

    [HttpGet("api/whatsapp/accounts/qr/{id}/{botId}")]
    public async Task<IActionResult> GetQr([FromRoute] Guid botId, Guid id)
    {
        var whatsAppAccountResponse = await whatsAppAccountService.GetById(User.GetId(), id);

        if (whatsAppAccountResponse == null)
            return BadRequest(new
            {
                message = "WhatsApp account is not found"
            });

        var qrResponse = await flowSellApi
            .GetQrAccount(webhookUrl + botId, whatsAppAccountResponse.IdInstance,
            whatsAppAccountResponse.ApiTokenInstance);

        return Ok(qrResponse);
    }
}