using Calabonga.UnitOfWork;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Infrastructure.Entities.Altegio;
using Microsoft.Extensions.Configuration;

namespace Copilot.Altegio.Api;

public class AltegioApiBuilder : IAltegioApiBuilder
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAltegioAPI _altegioApi;

    public AltegioApiBuilder(IConfiguration configuration, IUnitOfWork unitOfWork, IAltegioAPI altegioApi)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _altegioApi = altegioApi;
    }

    public async Task<IAltegioAPI> BuildByExternalCompanyIdAsync(int externalCompanyId)
    {
        var company = await _unitOfWork
            .GetRepository<AltegioCompany>()
            .GetFirstOrDefaultAsync(
                predicate: x => x.CompanyId == externalCompanyId
            );

        return GetAltegioApi(company);
    }

    private IAltegioAPI GetAltegioApi(AltegioCompany? company)
    {
        if (company == null) return _altegioApi;

        _altegioApi.SetDefaultParameters(
            partnerToken: _configuration["Altegio:PartnerToken"]!,
            userToken: _configuration["Altegio:UserToken"]!);

        return _altegioApi;
    }
}