using System.Net.Http.Headers;
using Copilot.Admin.Data.Services;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using System.Net.Http.Json;

namespace Copilot.Admin.Data.Apis;

public class ChatBotApi
{
    private readonly HttpClient _httpClient;
    private Guid _userId;

    public ChatBotApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public ChatBotApi SetDefaultParameters(UserClaims claims)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", claims.Token);
        _userId = claims.Id;

        return this;
    }
    
    public async Task<List<CopilotChatBotResponse>?> GetAll()
    {
        var response = await _httpClient.GetAsync($"api/chatbots?userId={_userId}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CopilotChatBotResponse>>();
    }

    public async Task<CopilotChatBotResponse?> GetById(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/chatbots/{id}?userId={_userId}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CopilotChatBotResponse>();
    }

    public async Task<CopilotChatBotResponse?> Create(CopilotChatBotRequest chatBotRequest)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/chatbots?userId={_userId}", chatBotRequest);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CopilotChatBotResponse>();
    }

    public async Task<CopilotChatBotResponse?> Update(Guid id, CopilotChatBotUpdateRequest chatBotRequest)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/chatbots/{id}", chatBotRequest);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CopilotChatBotResponse>();
    }

    public async Task Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/chatbots/{id}?userId={_userId}");

        response.EnsureSuccessStatusCode();
    }
}