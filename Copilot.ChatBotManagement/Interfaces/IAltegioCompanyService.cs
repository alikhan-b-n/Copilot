using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IAltegioCompanyService
{
    Task<List<AltegioCompanyResponse>> GetAll(Guid userId);

    Task<AltegioCompanyResponse?> GetById(Guid userId, Guid id);

    Task<bool> Create(AltegioCompanyResponse whatsAppAccountResponse);

    Task Delete(Guid id, Guid userId);
}