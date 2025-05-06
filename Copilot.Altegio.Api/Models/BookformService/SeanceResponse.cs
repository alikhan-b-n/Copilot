using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class SeanceResponse
{
    [JsonPropertyName("time")]
    public string Time { get; set; }
    
    [JsonPropertyName("seance_length")]
    public int? SeanceLength { get; set; }
    
    [JsonPropertyName("sum_length")]
    public int? SumLength { get; set; }
    
    [JsonPropertyName("datetime")]
    public string Datetime { get; set; }
}