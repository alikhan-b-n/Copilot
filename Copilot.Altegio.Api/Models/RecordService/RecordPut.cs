using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Copilot.Altegio.Api.Models.RecordService;

public class RecordPut
{
    [JsonProperty("attendance")]
    [JsonPropertyName("attendance")]
    public int Attendance { get; set; }

    [JsonProperty("staff_id")]
    [JsonPropertyName("staff_id")]
    public int StaffId { get; set; }

    [JsonProperty("seance_length")]
    [JsonPropertyName("seance_length")]
    public int SeanceLength { get; set; }

    [JsonProperty("datetime")]
    [JsonPropertyName("datetime")]
    public string Datetime { get; set; }

    [JsonProperty("client")]
    [JsonPropertyName("client")]
    public RecordPutClient Client { get; set; }

    [JsonProperty("activity_id")]
    [JsonPropertyName("activity_id")]
    public int ActivityId { get; set; }

    [JsonProperty("services")]
    [JsonPropertyName("services")]
    public IEnumerable<RecordPutService> Services { get; set; }

    [JsonProperty("clients_count")]
    [JsonPropertyName("clients_count")]
    public int ClientsCount { get; set; }
}

public class RecordPutService
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }
}

public class RecordPutClient
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonProperty("phone")]
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonProperty("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
}