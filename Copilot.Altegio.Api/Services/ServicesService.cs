using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.ServicesService;
using RestSharp;

namespace Copilot.Altegio.Api.Services;

public class ServicesService : IServicesService
{
    private readonly IRestClient _client;

    public ServicesService(IRestClient client)
    {
        _client = client;
    }

    public async Task<AltegioResponse<ServiceCategoryResponse>> GetServiceCategoriesAsync(int companyId,
        int? serviceCategoryId = null)
    {
        var request = new RestRequest($"company/{companyId}/service_categories/{serviceCategoryId}");
        var response = await _client.ExecuteGetAsync<AltegioResponse<ServiceCategoryResponse>>(request);
        return response.Data;
    }
}