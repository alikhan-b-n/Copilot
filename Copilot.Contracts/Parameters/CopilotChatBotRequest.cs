namespace Copilot.Contracts.Parameters;

public class CopilotChatBotRequest
{
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string PersonalityPrompt { get; set; } = string.Empty;
    public Guid GptModelId { get; set; }
}