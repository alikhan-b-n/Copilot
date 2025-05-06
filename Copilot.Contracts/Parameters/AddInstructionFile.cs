namespace Copilot.Contracts.Parameters;

public class AddInstructionFile
{
    public Guid BotId { get; set; }
    public Stream Content { get; set; }
    public string FileName { get; set; }
    public Guid UserId { get; set; }
}