using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.MessageHandling.Interfaces;
using Copilot.MessageHandling.Models;
using Copilot.Utils;
using Microsoft.Extensions.Caching.Distributed;

namespace Copilot.MessageHandling.Services;

/// <summary>
/// Represents a class for handling incoming messages.
/// </summary>
public class MessageHandler(
    IDistributedCache cache,
    IDialogueService dialogueService,
    IMessageSender messageSender) : IMessageHandler
{
    /// <summary>
    /// Handles incoming messages.
    /// </summary>
    /// <param name="botId">The ID of the bot.</param>
    /// <param name="model">The WhatsApp webhook model.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(Guid botId, WhatsappWebhookModel model)
    {
        MessageRole? role = model.TypeWebhook switch
        {
            TypeWebhook.IncomingMessageReceived => MessageRole.Client,
            _ => null
        };

        if (role is null) return;

        if (await IsNewEvent(model) == false) return;

        var idInstance = model.InstanceData?.IdInstance;
        if (idInstance is null) return;

        var phoneNumber = GetPhoneNumber(model);

        var request = new NewMessageDialogueModelRequest
        {
            ExternalIdentifier = phoneNumber,
            PhoneNumber = phoneNumber,
            MessageContent = model.GetTextMessage() ?? string.Empty,
            ChatBotId = botId
        };

        var result = await dialogueService.ProcessMessage(request);

        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }
        
        await messageSender.SendMessage(
            idInstance: (long)idInstance,
            chatId: phoneNumber,
            message: result
        );
    }

    /// <summary>
    /// Determines whether the event is new based on the given WhatsApp webhook model.
    /// </summary>
    /// <param name="model">The WhatsApp webhook model.</param>
    /// <returns>True if the event is new; otherwise, false.</returns>
    private async Task<bool> IsNewEvent(WhatsappWebhookModel model)
    {
        if (string.IsNullOrWhiteSpace(model.IdMessage))
        {
            return false;
        }

        var cacheKey = "copilot:webhook:event:" + model.IdMessage;

        var value = await cache.GetStringAsync(cacheKey);

        if (value is not null)
        {
            return false;
        }

        await cache.SetStringAsync(cacheKey, "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return true;
    }

    private string GetPhoneNumber(WhatsappWebhookModel model) =>
        PhoneCorrector.ConvertPhoneNumberToDigits(model.SenderData?.ChatId?[..model.SenderData.ChatId.IndexOf('@')]!);
}