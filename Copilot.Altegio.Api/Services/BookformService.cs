using System.Text.Json;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.BookformService;
using Copilot.Altegio.Api.Models.RecordService;
using RestSharp;

namespace Copilot.Altegio.Api.Services;

public class BookformService : IBookformService
{
    private readonly IRestClient _client;

    public BookformService(IRestClient client)
    {
        _client = client;
    }

    public async Task<AltegioSingleResponse<BookServicesResponse>> GetBookServicesAsync(int companyId,
        int? staffId = null, DateTimeOffset? date = null, int[] serviceIds = null)
    {
        var url = $"book_services/{companyId}";
        var request = new RestRequest(url);

        AddQueryParameters(request, staffId, date, serviceIds);

        var result =
            await _client.ExecuteGetAsync<AltegioSingleResponse<BookServicesResponse>>(request);
        return result.Data;
    }

    public async Task<AltegioResponse<SeanceResponse>> GetBookTimes(
        int companyId, int staffId, DateTimeOffset date, int[] serviceIds = null)
    {
        var url = $"book_times/{companyId}/{staffId}/{date:yyyyMMdd}";
        var request = new RestRequest(url);

        AddQueryParameters(request, null, null, serviceIds);

        var response = await _client.ExecuteGetAsync(request);
        var result = JsonSerializer.Deserialize<AltegioResponse<SeanceResponse>>(response.Content!);
        return result;
    }

    public async Task<AltegioMetaResponse<BookCheckMeta>> BookCheck(int companyId, BookCheckParameter parameter)
    {
        var url = $"book_check/{companyId}";
        var request = new RestRequest(url);
        request.AddJsonBody(parameter);

        var response = await _client.ExecutePostAsync(request);
        var result = JsonSerializer.Deserialize<AltegioMetaResponse<BookCheckMeta>>(response.Content!);
        return result;
    }

    public async Task<AltegioResponse<BookRecordResult>> BookRecord(int companyId, BookRecordParameter parameter)
    {
        var url = $"book_record/{companyId}";
        var request = new RestRequest(url);
        request.AddJsonBody(parameter);

        var response = await _client.ExecutePostAsync(request);
        var result = JsonSerializer.Deserialize<AltegioResponse<BookRecordResult>>(response.Content!);
        return result;
    }

    public async Task<AltegioSingleResponse<GetBookStuffSeancesResponse>> GetBookStuffSeances(int companyId,
        int staffId, DateTimeOffset? date = null, int[] serviceIds = null)
    {
        var url = $"book_staff_seances/{companyId}/{staffId}";
        var request = new RestRequest(url);

        AddQueryParameters(request, null, date, serviceIds);

        var response = await _client.ExecuteGetAsync(request);
        var result = JsonSerializer.Deserialize<AltegioSingleResponse<GetBookStuffSeancesResponse>>(response.Content!);
        return result;
    }

    private static void AddQueryParameters(RestRequest request, int? stuffId, DateTimeOffset? date, int[] serviceIds)
    {
        if (stuffId is not null) request.AddQueryParameter("staff_id", (int)stuffId);
        if (date is not null) request.AddQueryParameter("datetime", $"{date:yyyy-MM-ddTHH:mm}");
        if (serviceIds is not null)
            foreach (var serviceId in serviceIds)
                request.AddQueryParameter("service_ids", serviceId);
    }
}