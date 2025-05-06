using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace Copilot.Altegio.Api;

public class AltegioApi : IAltegioAPI
{
    private static readonly JsonSerializerSettings _serializationSettings = new()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    private string _partnerToken;
    private IRestClient _restClient;

    /// <summary>
    /// Represents the Altegio API client.
    /// </summary>
    public AltegioApi()
    {
    }

    public IRecordService RecordService { get; private set; }
    public IClientService ClientService { get; private set; }
    public IServicesService ServicesService { get; private set; }
    public IEmployeeService EmployeeService { get; private set; }
    public IBookformService BookformService { get; private set; }
    public IPartnerService PartnerService { get; private set; }

    /// <summary>
    /// Sets the default parameters for the Altegio API client.
    /// </summary>
    /// <param name="partnerToken">The partner token for authenticating with the Altegio API.</param>
    /// <param name="userToken">The user token for authenticating with the Altegio API.</param>
    public void SetDefaultParameters(string partnerToken, string userToken)
    {
        _partnerToken = partnerToken;
        var apiUrl = "https://app.alteg.io/api/v1";

        var options = new RestClientOptions(apiUrl)
        {
            RemoteCertificateValidationCallback = (_, _, _, _) => true,
            Authenticator = new JwtAuthenticator($"{_partnerToken}, User {userToken}"),
            ThrowOnDeserializationError = true
        };

        _restClient = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson(_serializationSettings));
        _restClient.AddDefaultHeader("Accept", "application/vnd.yclients.v2+json");

        InitServices();
    }

    private void InitServices()
    {
        RecordService = new RecordService(_restClient);

        ClientService = new ClientService(_restClient);
        ServicesService = new ServicesService(_restClient);
        EmployeeService = new EmployeeService(_restClient);
        BookformService = new BookformService(_restClient);
        PartnerService = new PartnerService();
    }
}