using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IDialoguesService
{
    Task<List<DialogueResponse>> GetAll(Guid userId, Guid botId, bool showTest);
    Task ChangeStatus(Guid userId, Guid botId, Guid dialogueId);
    Task Delete(Guid userId, Guid botId, Guid dialogueId);
}

