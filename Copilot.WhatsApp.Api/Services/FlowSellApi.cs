using System.Text.Json;
using System.Text.Json.Serialization;
using Copilot.Contracts.Responses;
using Copilot.WhatsApp.Api.Interfaces;
using RestSharp;

namespace Copilot.WhatsApp.Api.Services;

public class FlowSellApi : IFlowSellApi
{
    private const string Url = "https://dev.flowsell.me/api/v1";
    private const string PartnerToken = "80dae8d3-a87b-42ff-82ec-3a0468253a13";

    public FlowSellApi()
    {
    }

    public async Task<AccountGetSettingsResponse?> GetAccountSetting(int idInstance, string apiTokenInstance)
    {
        var request = new RestRequest($"{Url}/waInstance{idInstance}/getSettings/{apiTokenInstance}");

        request.AddStringBody(JsonSerializer.Serialize(
                value: new
                {
                },
                options: new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),
            DataFormat.Json);

        try
        {
            var restClient = new RestClient();

            RestResponse<AccountGetSettingsResponse> response =
                await restClient.ExecutePostAsync<AccountGetSettingsResponse>(request);

            return response.Data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<AccountGetQrResponse?> GetQrAccount(string webhookUrl, int idInstance, string apiTokenInstance)
    {
        var request = new RestRequest($"{Url}/waInstance{idInstance}/qr/{apiTokenInstance}");

        request.AddStringBody(JsonSerializer.Serialize(
                value: new
                {
                    webhookUrl
                },
                options: new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),
            DataFormat.Json);

        try
        {
            var restClient = new RestClient();

            RestResponse<AccountGetQrResponse> response =
                await restClient.ExecuteGetAsync<AccountGetQrResponse>(request);

            return response.Data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<CreateInstanceResponse> CreateInstance(string webhookUrl)
    {
        var request = new RestRequest($"{Url}/partner/createInstance/{PartnerToken}");

        request.AddStringBody(JsonSerializer.Serialize(
                value: new
                {
                    webhookUrl
                },
                options: new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),
            DataFormat.Json);

        try
        {
            var restClient = new RestClient();

            var response = await restClient.ExecutePostAsync<CreateInstanceResponse>(request);

            if (response is { IsSuccessful: true, Data: not null })
            {
                return response.Data;
            }

            return null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteInstance(int idInstance)
    {
        var request = new RestRequest($"{Url}/api/v1/partner/createInstance/{PartnerToken}");

        request.AddStringBody(JsonSerializer.Serialize(
                value: new
                {
                    idInstance
                },
                options: new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),
            DataFormat.Json);

        try
        {
            var restClient = new RestClient();

            var response = await restClient.ExecutePostAsync<DeleteInstanceAccountResponse>(request);

            if (response is { IsSuccessful: true, Data: not null })
            {
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Sends a message to a chat using the FlowSell API.
    /// </summary>
    /// <param name="idInstance">The ID of the instance.</param>
    /// <param name="apiTokenInstance">The API token of the instance.</param>
    /// <param name="chatId">The ID of the chat.</param>
    /// <param name="message">The message to be sent.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response from the API.</returns>
    public async Task<SendMessageResponse> SendMessage(
        long idInstance,
        string apiTokenInstance,
        string chatId,
        string message)
    {
        var request = new RestRequest($"{Url}/waInstance{idInstance}/sendMessage/{apiTokenInstance}");

        request.AddStringBody(JsonSerializer.Serialize(
                value: new
                {
                    chatId,
                    message
                },
                options: new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }),
            DataFormat.Json);

        try
        {
            var restClient = new RestClient();

            var response = await restClient.ExecutePostAsync<SendMessageResponse>(request);

            if (response is { IsSuccessful: true, Data: not null })
            {
                return response.Data;
            }

            return new SendMessageResponse
            {
                ErrorMessage = "Error: " + response.StatusCode
            };
        }
        catch (Exception e)
        {
            return new SendMessageResponse
            {
                ErrorMessage = "Error: " + e.Message
            };
        }
    }
}