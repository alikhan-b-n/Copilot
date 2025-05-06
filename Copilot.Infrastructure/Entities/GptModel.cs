namespace Copilot.Infrastructure.Entities;

public class GptModel : BaseEntity
{
    public string Name { get; set; }
    
    public virtual ICollection<CopilotChatBot> CopilotChatBots { get; set; }
}