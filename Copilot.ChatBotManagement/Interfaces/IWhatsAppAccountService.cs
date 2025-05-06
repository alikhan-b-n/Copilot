using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IWhatsAppAccountService
{
    Task<List<WhatsAppAccountResponse>> GetAllWhatsAppAccounts(Guid userId);

    Task<WhatsAppAccountResponse?> GetById(Guid userId, Guid id);

    Task<bool> Create(WhatsAppAccountResponse whatsAppAccountResponse, Guid userId);

    Task<WhatsAppAccountResponse> Delete(Guid id, Guid userId);
}