using System.Net.Http.Headers;
using System.Net.Http.Json;
using Copilot.Admin.Data.Services;
using Copilot.Contracts.Responses;

namespace Copilot.Admin.Data.Apis;

public class AltegioCompanyApi(HttpClient httpClient)
{
    private Guid _userId;

    public AltegioCompanyApi SetDefaultParameters(UserClaims claims)
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", claims.Token);
        _userId = claims.Id;

        return this;
    }

    public async Task<List<AltegioCompanyResponse>?> GetAll()
    {
        var response = await httpClient.GetAsync("api/altegio/companies");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<AltegioCompanyResponse>>();
    }

    public async Task Create(CreateAltegioCompanyParams param)
    {
        var response = await httpClient
            .PostAsJsonAsync(
            "api/altegio/companies",
            new
            {
                param.CompanyId,
                param.CompanyName
            });

        response.EnsureSuccessStatusCode();

        await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task Delete(Guid id)
    {
        await httpClient.DeleteAsync($"api/altegio/companies/{id}");
    }
}

public class CreateAltegioCompanyParams
{
    public long CompanyId { get; set; }

    public string CompanyName { get; set; }
}