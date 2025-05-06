using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class BookRecordResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("record_id")]
    public int RecordId { get; set; }

    [JsonPropertyName("record_hash")]
    public string RecordHash { get; set; }
}