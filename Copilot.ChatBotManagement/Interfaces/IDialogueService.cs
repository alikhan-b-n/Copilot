using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IDialogueService
{
    Task<string> ProcessMessage(NewMessageDialogueModelRequest request);
}