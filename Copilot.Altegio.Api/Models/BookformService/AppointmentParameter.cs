using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class AppointmentParameter
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("services")]
    public List<int> Services { get; set; }

    [JsonPropertyName("staff_id")]
    public int StaffId { get; set; }

    [JsonPropertyName("datetime")]
    public string Datetime { get; set; }
}