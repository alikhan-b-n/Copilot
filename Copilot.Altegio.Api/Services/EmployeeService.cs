using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.EmployeeService;
using RestSharp;

namespace Copilot.Altegio.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRestClient _client;

    public EmployeeService(IRestClient client)
    {
        _client = client;
    }

    public async Task<AltegioResponse<EmployeeResponse>> GetEmployeesAsync(int companyId)
    {
        var request = new RestRequest($"company/{companyId}/staff/");
        var response = await _client.ExecuteGetAsync<AltegioResponse<EmployeeResponse>>(request);
        return response.Data;
    }
}