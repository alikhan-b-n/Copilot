using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models;

public class AltegioResponse<T> where T : class
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; }
}