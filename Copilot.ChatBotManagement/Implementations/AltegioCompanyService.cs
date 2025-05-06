using Calabonga.UnitOfWork;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities.Altegio;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Experimental.Agents.Exceptions;

namespace Copilot.ChatBotManagement.Implementations;

public class AltegioCompanyService(IUnitOfWork<ApplicationContext> unitOfWork) : IAltegioCompanyService
{
    public async Task<List<AltegioCompanyResponse>> GetAll(Guid userId)
    {
        return await unitOfWork.GetRepository<AltegioCompany>().GetAll(selector: x => new AltegioCompanyResponse
            {
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                UserId = x.UserId,
                Id = x.Id
            },
            predicate: x => x.UserId == userId,
            ignoreAutoIncludes: false
            )
            .ToListAsync();
    }

    public async Task<AltegioCompanyResponse?> GetById(Guid userId, Guid id)
    {
        var response = await unitOfWork.GetRepository<AltegioCompany>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == userId && x.Id == id)
            .Select(x => new AltegioCompanyResponse
            {
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                UserId = x.UserId,
                Id = x.Id
            })
            .FirstOrDefaultAsync();

        return response;
    }

    public async Task<bool> Create(AltegioCompanyResponse altegioCompanyResponse)
    {
        await unitOfWork.GetRepository<AltegioCompany>().InsertAsync(new AltegioCompany
        {
            CompanyId = altegioCompanyResponse.CompanyId,
            CompanyName = altegioCompanyResponse.CompanyName,
            UserId = altegioCompanyResponse.UserId
        });

        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task Delete(Guid id, Guid userId)
    {
        var response = await unitOfWork
            .GetRepository<AltegioCompany>()
            .GetAll(disableTracking: true)
            .Where(x => x.Id == id && x.UserId == userId)
            .FirstOrDefaultAsync();

        try
        {
            if (response == null)
                throw new ArgumentException("Not found", nameof(AltegioCompany));

            unitOfWork.GetRepository<AltegioCompany>().Delete(response.Id);

            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new AgentException(e.Message);
        }
    }
}