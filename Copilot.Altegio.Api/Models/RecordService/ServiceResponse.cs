using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.RecordService;

public class ServiceResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("cost")]
    public decimal Cost { get; set; }

    [JsonPropertyName("manual_cost")]
    public decimal ManualCost { get; set; }

    [JsonPropertyName("cost_per_unit")]
    public decimal CostPerUnit { get; set; }

    [JsonPropertyName("discount")]
    public decimal Discount { get; set; }

    [JsonPropertyName("first_cost")]
    public decimal FirstCost { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}