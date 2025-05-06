using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class GetBookStuffSeancesResponse
{
    [JsonPropertyName("seance_date")]
    public string SeanceDate { get; set; }
    
    [JsonPropertyName("seances")]
    public List<SeanceResponse> Seances { get; set; }
}