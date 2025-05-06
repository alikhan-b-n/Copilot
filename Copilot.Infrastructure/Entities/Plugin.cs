namespace Copilot.Infrastructure.Entities;

public class Plugin : BaseEntity
{
    public string Name { get; set; }
    public string ParameterData { get; set; } = string.Empty;

    public Guid CopilotChatBotId { get; set; }
    public CopilotChatBot CopilotChatBot { get; set; }
}