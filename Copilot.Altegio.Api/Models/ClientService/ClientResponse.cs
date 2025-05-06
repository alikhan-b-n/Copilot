using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.ClientService;

public class ClientResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
