using System.Security.AccessControl;

namespace Copilot.Contracts.Responses;

public class CopilotChatBotResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string PersonalityPrompt { get; set; }
    public int Timezone { get; set; }
    public Guid GptModelId { get; set; }
    public Guid UserId { get; set; }
    public List<Guid?> InstructionFilesIds { get; set; }
}