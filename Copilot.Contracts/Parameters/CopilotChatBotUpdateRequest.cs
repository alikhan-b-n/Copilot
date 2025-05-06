namespace Copilot.Contracts.Parameters;

public class CopilotChatBotUpdateRequest
{
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? PersonalityPrompt { get; set; }
    public int? Timezone { get; set; }
    public Guid? GptModelId { get; set; }
}