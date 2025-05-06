using System.Text.Json.Serialization;
using Copilot.Contracts.Responses;
using RestSharp;

namespace Copilot.WhatsApp.Api.Interfaces;

public interface IFlowSellApi
{
    public Task<AccountGetSettingsResponse?> GetAccountSetting(int idInstance, string apiTokenInstance);

    public Task<AccountGetQrResponse?> GetQrAccount(string webhookUrl, int idInstance, string apiTokenInstance);

    public Task<CreateInstanceResponse> CreateInstance(string webhookUrl);

    public Task<bool> DeleteInstance(int idInstance);
    
    Task<SendMessageResponse> SendMessage(long idInstance, string apiTokenInstance, string chatId, string message);
}

public class SendMessageResponse
{
    [JsonPropertyName("idMessage")]
    public string? IdMessage { get; set; }
    
    [JsonPropertyName("description")]
    public string? ErrorMessage { get; set; }

    public bool IsError()
    {
        return ErrorMessage is null;
    }
}