using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface ICopilotChatBotService
{
    Task<List<CopilotChatBotResponse>> GetAllChatBots(Guid userId);
    Task<CopilotChatBotResponse> GetChatBotById(Guid userId, Guid id);
    Task<CopilotChatBotResponse> CreateChatBot(Guid userId, CopilotChatBotRequest chatBot);
    Task<CopilotChatBotResponse> UpdateChatBot(Guid id, Guid userId, CopilotChatBotUpdateRequest chatBot);
    Task DeleteChatBot(Guid id, Guid userId);
}