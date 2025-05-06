namespace Copilot.Infrastructure.Entities;

public class CopilotChatBot : BaseEntity
{
    public string AssistantId { get; set; }
    public string Title { get; set; }
    public int Timezone { get; set; }
    public ChatBotStatus Status { get; set; }
    public string PersonalityPrompt { get; set; }

    public List<InstructionFile>? InstructionFiles { get; set; }

    public List<Dialogue>? Dialogues { get; set; }

    public Guid GptModelId { get; set; } = DefaultDataConstants.Gpt35TurboModel.Id;
    public virtual GptModel GptModel { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}