using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.PartnerService;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace Copilot.Altegio.Api.Services;

public class PartnerService : IPartnerService
{
    private readonly IRestClient _marketplaceClient;

    public PartnerService()
    {
        var apiUrl = "https://app.alteg.io";
        var options = new RestClientOptions(apiUrl)
        {
            Authenticator = new JwtAuthenticator("xbrd2ac9kgg4a9fkuyxe"),
            ThrowOnDeserializationError = true
        };

        var serializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        _marketplaceClient = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson(serializerSettings));
    }

    public async Task<AltegioSingleResponse<ConnectionStatusResponse>> ConnectionStatusAsync(int salonId,
        int applicationId)
    {
        var request = new RestRequest($"marketplace/salon/{salonId}/application/{applicationId}");

        var response = await _marketplaceClient.ExecuteGetAsync<AltegioSingleResponse<ConnectionStatusResponse>>(request);
        return response.Data;
    }
}
