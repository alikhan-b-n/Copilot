namespace Copilot.Contracts.Responses;

public class InstructionFileResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public Guid UserId { get; set; }
    public Guid BotId { get; set; }
}