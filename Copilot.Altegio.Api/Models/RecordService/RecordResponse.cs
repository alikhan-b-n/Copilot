using System.Text.Json.Serialization;

namespace Copilot.Altegio.Api.Models.RecordService;

public class RecordResponse : IEquatable<RecordResponse>
{
    public RecordResponse()
    {
    }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("company_id")]
    public int CompanyId { get; set; }

    [JsonPropertyName("staff_id")]
    public int StaffId { get; set; }

    [JsonPropertyName("services")]
    public IEnumerable<ServiceResponse> Services { get; set; }
    

    [JsonPropertyName("client")]
    public ClientResponse Client { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("datetime")]
    public string DateTime { get; set; }

    [JsonPropertyName("seance_length")]
    public int SeanceLength { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("visit_id")]
    public int VisitId { get; set; }

    [JsonPropertyName("create_date")]
    public string CreateDate { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonPropertyName("confirmed")]
    public int Confirmed { get; set; }

    [JsonPropertyName("online")]
    public bool Online { get; set; }

    [JsonPropertyName("visit_attendance")]
    public int VisitAttendance { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("activity_id")]
    public int ActivityId { get; set; }

    [JsonPropertyName("last_change_date")]
    public string LastChangeDate { get; set; }

    /// <summary>
    /// Represents the attendance status of a record.
    /// </summary>
    [JsonPropertyName("attendance")]
    public int Attendance { get; set; }

    [JsonPropertyName("short_link")]
    public string ShortLink { get; set; }

    public bool Equals(RecordResponse other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public string GetShortToken()
    {
        if (string.IsNullOrEmpty(ShortLink))
            return null;

        var tokens = ShortLink.Split('/');
        return tokens.Length < 5 ? null : tokens[4];
    }
}