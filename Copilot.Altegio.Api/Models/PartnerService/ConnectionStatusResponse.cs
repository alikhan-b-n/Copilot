using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.PartnerService;

public class ConnectionStatusResponse
{
    [JsonPropertyName("connection_status")]
    public ConnectionStatus ConnectionStatus { get; set; }
}

public class ConnectionStatus
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
}