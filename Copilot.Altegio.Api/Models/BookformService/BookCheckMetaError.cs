using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class BookCheckMetaError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}