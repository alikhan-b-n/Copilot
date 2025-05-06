using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Copilot.Admin.Data.Exceptions;
using Copilot.Admin.Data.Services;
using Copilot.Contracts.Responses;

namespace Copilot.Admin.Data.Apis;

public class GptModelApi
{
    private readonly HttpClient _httpClient;

    public GptModelApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public GptModelApi SetDefaultParameters(UserClaims claims)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", claims.Token);

        return this;
    }

    public async Task<List<GptModelResponse>?> GetAll()
    {
        var response = await _httpClient.GetAsync("/api/gptmodels");

        return response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<List<GptModelResponse>>(),

            HttpStatusCode.Unauthorized =>
                throw new UnauthorizedHttpException(await response.Content.ReadAsStringAsync()),

            _ => throw new UnsupportedHttpException(await response.Content.ReadAsStringAsync())
        };
    }
}