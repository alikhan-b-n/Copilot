using Calabonga.UnitOfWork;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Copilot.ChatBotManagement.Implementations;

public class GptModelService(IUnitOfWork<ApplicationContext> unitOfWork) : IGptModelService
{
    public async Task<List<GptModelResponse>> GetAllGptModels()
    {
        var gptmodels = await unitOfWork
            .GetRepository<GptModel>()
            .GetAll(disableTracking: true)
            .Select(x => new GptModelResponse()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
        
        return gptmodels;
    }
}