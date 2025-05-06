using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.BookformService;

public class BookRecordParameter
{
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("fullname")]
    public string Fullname { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonPropertyName("appointments")]
    public List<Appointment> Appointments { get; set; }
    
    public class Appointment
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
}