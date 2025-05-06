using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class BookCheckMeta
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("errors")]
    public List<BookCheckMetaError> Errors { get; set; }
}