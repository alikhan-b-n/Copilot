using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.ClientService;
using RestSharp;

namespace Copilot.Altegio.Api.Services;

public class ClientService : IClientService
{
    private readonly IRestClient _client;

    public ClientService(IRestClient client)
    {
        _client = client;
    }

    public async Task<AltegioResponse<ClientResponse>> GetClientsAsync(
        int companyId,
        int? count = null,
        int? page = null,
        string fullname = null,
        string phone = null,
        string email = null)
    {
        var request = new RestRequest($"clients/{companyId}");
        if (count.HasValue) request.AddParameter("count", count.Value);
        if (page.HasValue) request.AddParameter("page", page.Value);
        request.AddParameter("full_name", fullname);
        request.AddParameter("phone", phone);
        request.AddParameter("email", email);

        return await _client.GetAsync<AltegioResponse<ClientResponse>>(request);
    }
}