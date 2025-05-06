using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Copilot.Altegio.Api.Models.BookformService;

public class BookServicesResponse
{
    [JsonProperty("services")]
    [JsonPropertyName("services")]
    public List<BookService> Services { get; set; }
}

public class BookService
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonProperty("category_id")]
    [JsonPropertyName("category_id")]
    public int? CategoryId { get; set; }

    [JsonProperty("price_min")]
    [JsonPropertyName("price_min")]
    public int? PriceMin { get; set; }

    [JsonProperty("price_max")]
    [JsonPropertyName("price_max")]
    public int? PriceMax { get; set; }

    [JsonProperty("discount")]
    [JsonPropertyName("discount")]
    public int? Discount { get; set; }

    [JsonProperty("comment")]
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonProperty("weight")]
    [JsonPropertyName("weight")]
    public int? Weight { get; set; }

    [JsonProperty("active")]
    [JsonPropertyName("active")]
    public int? Active { get; set; }

    [JsonProperty("sex")]
    [JsonPropertyName("sex")]
    public int? Sex { get; set; }

    [JsonProperty("image")]
    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonProperty("prepaid")]
    [JsonPropertyName("prepaid")]
    public string Prepaid { get; set; }

    [JsonProperty("seance_length")]
    [JsonPropertyName("seance_length")]
    public int? SeanceLength { get; set; }
}