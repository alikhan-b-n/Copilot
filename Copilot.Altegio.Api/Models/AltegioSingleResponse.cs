using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models;

public class AltegioSingleResponse<T> where T : class, new()
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; } = new();

    [JsonPropertyName("meta")]
    public object Meta { get; set; }
}

public class AltegioMetaResponse<TMeta> where TMeta : new()
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("meta")] 
    public TMeta Meta { get; set; } = new();
}