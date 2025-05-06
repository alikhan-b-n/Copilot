namespace Copilot.Infrastructure.Entities;

public class InstructionFile : BaseEntity
{
    public string Url { get; set; }

    public string FileName { get; set; }

    public Guid UserId { get; set; }

    public Guid BotId { get; set; }
}