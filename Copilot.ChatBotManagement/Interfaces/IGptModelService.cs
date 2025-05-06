using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IGptModelService
{
    Task<List<GptModelResponse>> GetAllGptModels();
}
