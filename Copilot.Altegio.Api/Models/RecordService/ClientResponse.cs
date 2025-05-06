using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.RecordService;

public class ClientResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("surname")]
    public string Surname { get; set; }
    
    [JsonPropertyName("patronymic")]
    public string Patronymic { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("card")]
    public string Card { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("success_visits_count")]
    public int SuccessVisitsCount { get; set; }

    [JsonPropertyName("fail_visits_count")]
    public int FailVisitsCount { get; set; }
}