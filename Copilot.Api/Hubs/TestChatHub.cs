using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Microsoft.AspNetCore.SignalR;

namespace Copilot.Api.Hubs;

public class TestChatHub(IDialogueService dialogueService) : Hub
{
    public async Task SendMessage(Guid botId, string conversationId, string message)
    {
        var answer = await dialogueService.ProcessMessage(
            new NewMessageDialogueModelRequest
            {
                ExternalIdentifier = $"test_{conversationId}",
                MessageContent = message,
                ChatBotId = botId
            }
        );

        await Clients.All.SendAsync("ReceiveMessage", answer);
    }
}