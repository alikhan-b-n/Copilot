using System.Net.Http.Headers;
using System.Net.Http.Json;
using Copilot.Admin.Data.Services;
using Copilot.Contracts.Responses;

namespace Copilot.Admin.Data.Apis;

public class WhatsAppApi(HttpClient httpClient)
{
    private Guid _userId;

    public WhatsAppApi SetDefaultParameters(UserClaims claims)
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", claims.Token);
        _userId = claims.Id;

        return this;
    }

    public async Task<List<WhatsAppAccountResponse>?> GetAllWhatsAppAccounts()
    {
        var response = await httpClient.GetAsync($"api/whatsapp/accounts");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<WhatsAppAccountResponse>>();
    }

    public async Task<AccountGetSettingsResponse?> GetAccountSetting(Guid accountId)
    {
        var responseMessage = await httpClient.GetAsync($"api/whatsapp/accounts/{accountId}");

        return await responseMessage.Content.ReadFromJsonAsync<AccountGetSettingsResponse>();
    }

    public async Task<string> GetQrCodeForPhoneNumberBinding(Guid accountId, Guid botId)
    {
        var response = await httpClient.GetAsync($"api/whatsapp/accounts/qr/{accountId}/{botId}");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AccountGetQrResponse>();

        if (result.Type == "qrCode")
        {
            return result.Message;
        }

        return "Already binded WhatsApp account";
    }

    public async Task<bool> CreateInstance(CreateWhatsAppAccountParams createWhatsAppAccountParams)
    {
        var response = await httpClient
            .PostAsJsonAsync(
            $"api/whatsapp/accounts/settings/{createWhatsAppAccountParams.BotId}",
            new
            {
                createWhatsAppAccountParams.PhoneNumber
            });

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task DeleteInstance(Guid id)
    {
        await httpClient.DeleteAsync($"api/whatsapp/accounts/settings/{id}");
    }
}

public class CreateWhatsAppAccountParams
{
    public Guid BotId { get; set; }

    public string PhoneNumber { get; set; }
}