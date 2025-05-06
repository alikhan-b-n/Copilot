using Copilot.MessageHandling.Interfaces;
using Copilot.MessageHandling.Models;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

[Controller]
public class WhatsAppWebhookController(IMessageHandler messageHandler) : ControllerBase
{
    [HttpPost("/api/whatsapp/{botId:guid}")]
    public async Task<IActionResult> Handle(Guid botId, [FromBody] WhatsappWebhookModel webhookModel)
    {
        try
        {
            await messageHandler.Handle(botId, webhookModel);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }

        return Ok();
    }
}