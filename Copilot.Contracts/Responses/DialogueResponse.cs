namespace Copilot.Contracts.Responses;

public class DialogueResponse
{
    public Guid Id { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}