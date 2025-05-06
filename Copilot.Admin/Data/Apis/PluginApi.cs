using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Copilot.Admin.Data.Exceptions;
using Copilot.Admin.Data.Services;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;

namespace Copilot.Admin.Data.Apis;

public class PluginApi
{
    private readonly HttpClient _httpClient;

    public PluginApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public PluginApi SetDefaultParameters(UserClaims claims)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", claims.Token);

        return this;
    }

    public async Task<AllPluginsModel?> GetAll(Guid botId)
    {
        var response = await _httpClient.GetAsync($"api/chatbots/{botId}/plugins");

        return response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<AllPluginsModel>(),

            HttpStatusCode.Unauthorized =>
                throw new UnauthorizedHttpException(await response.Content.ReadAsStringAsync()),

            _ => throw new UnsupportedHttpException(await response.Content.ReadAsStringAsync())
        };
    }

    public async Task UpsertSalonPlugin(Guid botId, SalonPluginParameter parameter)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/chatbots/{botId}/plugins/salon", parameter);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpsertOperatorPlugin(Guid botId, OperatorPluginParameter parameter)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/chatbots/{botId}/plugins/operator", parameter);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePlugin(Guid botId, Guid pluginId)
    {
        var response = await _httpClient.DeleteAsync($"api/chatbots/{botId}/plugins/{pluginId}");

        response.EnsureSuccessStatusCode();
    }
}