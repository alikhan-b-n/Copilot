namespace Copilot.Ai.Utils.Options;

public class OpenAiModelOptions
{
    public string EmbeddingModelId { get; set; } = null!;
    public string CompletionModelId { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}