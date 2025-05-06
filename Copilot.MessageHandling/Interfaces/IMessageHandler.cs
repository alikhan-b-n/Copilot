using Copilot.MessageHandling.Models;

namespace Copilot.MessageHandling.Interfaces;

public interface IMessageHandler
{
    Task Handle(Guid botId, WhatsappWebhookModel model);
}