using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IFaqService
{
    Task<List<FaqResponse>> GetAllFaqs(Guid userId);
    Task<FaqResponse> GetFaqById(Guid userId, Guid id);
    Task<FaqResponse> CreateFaq(Guid userId, FaqRequest faqRequest);
    Task<FaqResponse> UpdateFaq(Guid id, Guid userId, FaqRequest faqRequest);
    Task DeleteFaq(Guid id, Guid userId);
}