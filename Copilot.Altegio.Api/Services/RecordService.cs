using System.Net;
using System.Security.Authentication;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.RecordService;
using Newtonsoft.Json;
using RestSharp;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Copilot.Altegio.Api.Services;

public class RecordService : IRecordService
{
    private readonly IRestClient _client;

    public RecordService(IRestClient client)
    {
        _client = client;
    }

    public async Task<AltegioSingleResponse<RecordResponse>> GetRecordAsync(int companyId, int recordId)
    {
        var request = new RestRequest($"record/{companyId}/{recordId}");
        request.AddParameter("short_link_token", 1);
        var response = await _client.ExecuteGetAsync(request);

        try
        {
            return JsonSerializer.Deserialize<AltegioSingleResponse<RecordResponse>>(response.Content ?? string.Empty);
        }
        catch (Exception e)
        {
            throw new Exception(response.Content, e);
        }
    }

    public async Task<AltegioResponse<RecordResponse>> GetRecordsAsync(
        int companyId,
        int? page = null,
        int? count = null,
        int? staffId = null,
        int? clientId = null,
        string startDate = null,
        string endDate = null,
        string cStartDate = null,
        string cEndDate = null,
        string changedAfter = null,
        string changedBefore = null,
        int includeConsumables = 0,
        int includeFinanceTransactions = 0)
    {
        var request = new RestRequest($"records/{companyId}");
        AddParameterIfNotNull(request, "page", page);
        AddParameterIfNotNull(request, "count", count);
        AddParameterIfNotNull(request, "staff_id", staffId);
        AddParameterIfNotNull(request, "client_id", clientId);
        request.AddParameter("start_date", startDate);
        request.AddParameter("end_date", endDate);
        request.AddParameter("c_start_date", cStartDate);
        request.AddParameter("c_end_date", cEndDate);
        request.AddParameter("changed_after", changedAfter);
        request.AddParameter("changed_before", changedBefore);
        request.AddParameter("include_consumables", includeConsumables);
        request.AddParameter("inclide_finance_transactions", includeFinanceTransactions);

        var response = await _client.ExecuteGetAsync(request);

        if (!response.IsSuccessful)
        {
            throw response.StatusCode switch
            {
                HttpStatusCode.Forbidden => new AuthenticationException(response.Content, response.ErrorException),
                _ => new Exception(response.Content, response.ErrorException)
            };
        }

        try
        {
            var result = JsonSerializer.Deserialize<AltegioResponse<RecordResponse>>(response.Content);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(response.Content, e);
        }
    }

    public async Task<AltegioSingleResponse<RecordResponse>> EditRecordAsync(int companyId, int recordId,
        RecordPut record)
    {
        var request = new RestRequest($"record/{companyId}/{recordId}");
        var json = JsonConvert.SerializeObject(record, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        request.AddStringBody(json, DataFormat.Json);

        var response = await _client.ExecutePutAsync<AltegioSingleResponse<RecordResponse>>(request);
        return response.Data;
    }

    public async Task DeleteRecordAsync(int companyId, int recordId)
    {
        var request = new RestRequest($"record/{companyId}/{recordId}", Method.Delete);

        await _client.ExecuteAsync(request);
    }

    private void AddParameterIfNotNull(RestRequest request, string parameterName, int? parameterValue)
    {
        if (parameterValue.HasValue)
            request.AddParameter(parameterName, parameterValue.Value);
    }
}