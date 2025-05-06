using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Copilot.Admin.Data.Exceptions;
using Copilot.Admin.Data.Services;
using Copilot.Contracts.Responses;

namespace Copilot.Admin.Data.Apis;

public class DialoguesApi
{
    private readonly HttpClient _httpClient;

    public DialoguesApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public DialoguesApi SetDefaultParameters(UserClaims claims)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", claims.Token);

        return this;
    }
    
    public async Task<List<DialogueResponse>?> GetAll(Guid botId)
    {
        var response = await _httpClient.GetAsync($"api/chatbots/{botId}/dialogues");

        return response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<List<DialogueResponse>>(),

            HttpStatusCode.Unauthorized =>
                throw new UnauthorizedHttpException(await response.Content.ReadAsStringAsync()),

            _ => throw new UnsupportedHttpException(await response.Content.ReadAsStringAsync())
        };
    }
    
    public async Task ChangeStatus(Guid botId, Guid dialogueId)
    {
        var response = await _httpClient.PatchAsync($"api/chatbots/{botId}/dialogues/{dialogueId}", null);

        response.EnsureSuccessStatusCode();
    }
    
    public async Task Delete(Guid botId, Guid dialogueId)
    {
        var response = await _httpClient.DeleteAsync($"api/chatbots/{botId}/dialogues/{dialogueId}");

        response.EnsureSuccessStatusCode();
    }
}