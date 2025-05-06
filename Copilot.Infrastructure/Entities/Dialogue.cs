namespace Copilot.Infrastructure.Entities;

public class Dialogue : BaseEntity
{
    public required string ThreadId { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;
    
    public Guid CopilotChatBotId { get; set; }
    public virtual CopilotChatBot CopilotChatBot { get; set; }

    public string ExternalIdentifier { get; set; }
}