using Azure.AI.OpenAI;
using Copilot.Ai.Utils.Interfaces;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Copilot.Ai.Utils.Services;

public class GptTextEmitter : IGptTextEmitter
{
    private readonly IKernelFactory _kernelFactory;

    public GptTextEmitter(IKernelFactory kernelFactory)
    {
        _kernelFactory = kernelFactory;
    }

    public async Task<ChatBotResponseModel> Emmit(
        string prompt,
        string gptModel = IGptTextEmitter.DefaultGptModel,
        double temperature = IGptTextEmitter.DefaultTemperature,
        int maxTokens = IGptTextEmitter.DefaultMaxTokens,
        CancellationToken cancellationToken = default)
    {
        var kernel = _kernelFactory.Create(gptModel);

        var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();
        var chatHistory = new ChatHistory
        {
            new(AuthorRole.User, prompt)
        };

        var results = await chatCompletion.GetChatMessageContentsAsync(chatHistory,
            new OpenAIPromptExecutionSettings
            {
                ModelId = gptModel,
                Temperature = temperature,
                MaxTokens = maxTokens,
                ResultsPerPrompt = 1
            },
            cancellationToken: cancellationToken);

        var message = results.First() as OpenAIChatMessageContent;

        var content = message!.Content ?? string.Empty;

        if (message.Metadata!.TryGetValue("Usage", out var item)
            && item is CompletionsUsage usage)
        {
            return new ChatBotResponseModel(content, new ChatBotResponseUsage
            {
                CompletionTokens = usage.CompletionTokens,
                PromptTokens = usage.PromptTokens
            });
        }

        return new ChatBotResponseModel(content);
    }
}