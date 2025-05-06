namespace Copilot.Ai.Models;

public record AiResponse(string Content, decimal PromptTokens, decimal AnswerTokens)
{
    public static readonly AiResponse Empty = new(string.Empty, 0, 0);
}