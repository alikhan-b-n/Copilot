using Calabonga.UnitOfWork;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Experimental.Agents.Exceptions;

namespace Copilot.ChatBotManagement.Implementations;

public class WhatsAppAccountService(IUnitOfWork<ApplicationContext> unitOfWork, ICopilotChatBotService copilotChatBotService) : IWhatsAppAccountService
{
    public async Task<List<WhatsAppAccountResponse>> GetAllWhatsAppAccounts(Guid userId)
    {
        var result = await unitOfWork
            .GetRepository<WhatsAppAccount>()
            .GetAll(
            selector: x => new WhatsAppAccountResponse
            {
                ApiTokenInstance = x.ApiTokenInstance,
                IdInstance = x.IdInstance,
                PhoneNumber = x.PhoneNumber,
                Id = x.Id,
                BotId = x.BotId,
            },
            predicate: x => x.UserId == userId,
            ignoreAutoIncludes: false
            )
            .ToListAsync();

        foreach (var whatsAppAccountResponse in result)
        {
            var bot = await copilotChatBotService.GetChatBotById(userId, whatsAppAccountResponse.BotId);

            whatsAppAccountResponse.BotTitle = bot.Title;
        }

        return result;
    }

    public async Task<WhatsAppAccountResponse?> GetById(Guid userId, Guid id)
    {
        var whatsappInfo = await unitOfWork
            .GetRepository<WhatsAppAccount>()
            .GetAll(disableTracking: true)
            .Where(x => x.Id == id && x.UserId == userId)
            .Select(x => new WhatsAppAccountResponse
            {
                ApiTokenInstance = x.ApiTokenInstance,
                IdInstance = x.IdInstance,
                PhoneNumber = x.PhoneNumber,
                Id = x.Id,
                BotId = x.BotId
            })
            .FirstOrDefaultAsync();

        return whatsappInfo;
    }

    public async Task<bool> Create(WhatsAppAccountResponse whatsAppAccountResponse, Guid userId)
    {
        await unitOfWork.GetRepository<WhatsAppAccount>().InsertAsync(new WhatsAppAccount
        {
            ApiTokenInstance = whatsAppAccountResponse.ApiTokenInstance,
            IdInstance = whatsAppAccountResponse.IdInstance,
            PhoneNumber = whatsAppAccountResponse.PhoneNumber,
            UserId = userId,
            BotId = whatsAppAccountResponse.BotId
        });

        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<WhatsAppAccountResponse> Delete(Guid id, Guid userId)
    {
        var whatsAppAccountResponse = await unitOfWork
            .GetRepository<WhatsAppAccount>()
            .GetAll(disableTracking: true)
            .Where(x => x.Id == id && x.UserId == userId)
            .Select(x => new WhatsAppAccountResponse
            {
                ApiTokenInstance = x.ApiTokenInstance,
                IdInstance = x.IdInstance,
                PhoneNumber = x.PhoneNumber,
                Id = x.Id,
                BotId = x.BotId
            })
            .FirstOrDefaultAsync();

        try
        {
            if (whatsAppAccountResponse == null)
                throw new ArgumentException("Not found", nameof(WhatsAppAccount));

            unitOfWork.GetRepository<WhatsAppAccount>().Delete(whatsAppAccountResponse.Id);

            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new AgentException(e.Message);
        }

        return whatsAppAccountResponse;
    }
}