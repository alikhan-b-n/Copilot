using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class BookCheckParameter
{
    [JsonPropertyName("appointments")]
    public List<AppointmentParameter> Appointments { get; set; }
}