namespace Copilot.Contracts.Responses;

public class FaqResponse
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public Guid UserId { get; set; }
}