namespace Copilot.Ai.Utils.Interfaces;

/// <summary>
/// Service requests text generation to ChatGPT
/// </summary>
public interface IGptTextEmitter
{
    const string DefaultGptModel = "gpt-3.5-turbo";
    const double DefaultTemperature = .7;
    const int DefaultMaxTokens = 1000;
    
    Task<ChatBotResponseModel> Emmit(
        string prompt, 
        string gptModel = DefaultGptModel, 
        double temperature = DefaultTemperature, 
        int maxTokens = DefaultMaxTokens,
        CancellationToken cancellationToken = default);
}

public record ChatBotResponseModel(string MessageContent, ChatBotResponseUsage? Usage = default)
{
    public static readonly ChatBotResponseModel Empty = new(string.Empty);
}

public class ChatBotResponseUsage
{
    public int CompletionTokens { get; set; } = 0;
    public int PromptTokens { get; set; } = 0;
};