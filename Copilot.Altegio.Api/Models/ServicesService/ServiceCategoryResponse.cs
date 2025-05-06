using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.ServicesService;

public class ServiceCategoryResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("api_id")]
    public string ApiId { get; set; }

    [JsonPropertyName("weight")]
    public double Weight { get; set; }

    [JsonPropertyName("staff")]
    public IEnumerable<int> Staff { get; set; }
}