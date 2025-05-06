using Copilot.MessageHandling.Interfaces;
using Copilot.Utils;
using Copilot.WhatsApp.Api.Interfaces;

namespace Copilot.MessageHandling.Services;

public class MessageSender(IFlowSellApi flowSellApi) : IMessageSender
{
    // TODO: Get tokens of specific user from Db. 
    private readonly Dictionary<long, string> _instances = new()
    {
        { 33233, "dc4d39b6-ccf6-49e7-ba14-861be164e55a" }
    };

    /// <summary>
    /// Sends a WhatsApp message using the FlowSell API.
    /// </summary>
    /// <param name="idInstance">The identifier of the instance.</param>
    /// <param name="chatId">The chat ID where the message will be sent.</param>
    /// <param name="message">The content of the message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendMessage(long idInstance, string chatId, string message)
    {
        if (!_instances.TryGetValue(idInstance, out var tokenInstance))
            return;

        chatId = PhoneCorrector.ConvertPhoneNumberToDigits(chatId);
        
        var response = await flowSellApi.SendMessage(idInstance, tokenInstance, chatId, message);
        if (!response.IsError())
        {
            // Handle error
        }
    }
}