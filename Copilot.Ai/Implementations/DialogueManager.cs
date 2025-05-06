using Copilot.Ai.Interfaces;
using Copilot.Ai.Models;
using Copilot.AI.Plugins;
using Copilot.Infrastructure.Entities;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Copilot.Ai.Implementations;

/// <inheritdoc />
public class DialogueManager : IDialogueManager
{
    private readonly IAssistantRepository _assistantRepository;
    private readonly IServiceProvider _serviceProvider;

    public DialogueManager(IAssistantRepository assistantRepository, IServiceProvider serviceProvider)
    {
        _assistantRepository = assistantRepository;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task<AiResponse> GiveAnswer(DialogueManagerParameter parameter)
    {
        if (string.IsNullOrWhiteSpace(parameter.Dialogue.CopilotChatBot.GptModel.Name))
            throw new ArgumentException("No Model selected.");

        return await RenderAnswer(parameter);
    }

    private async Task<AiResponse> RenderAnswer(DialogueManagerParameter parameter)
    {
        var chatBot = parameter.Dialogue.CopilotChatBot;

        if (string.IsNullOrEmpty(chatBot.AssistantId))
            return AiResponse.Empty;

        var assistant = await _assistantRepository.GetAsync(
            assistantId: chatBot.AssistantId,
            plugins: GetPlugins(parameter)
        );

        var thread = await _assistantRepository.GetThreadAsync(
            chatBot.AssistantId,
            parameter.Dialogue.ThreadId
        );

        var prompt = RenderPrompt(chatBot);
        
        var promptExecutionSettings = new OpenAIPromptExecutionSettings
        {
            ModelId = chatBot.GptModel.Name,
            ChatSystemPrompt = prompt,
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var args = new KernelArguments(promptExecutionSettings)
        {
            [PromptConstants.PromptKey] = prompt
        };

        var response = await thread
            .InvokeAsync(assistant, parameter.IncomingMessage, args)
            .ToListAsync();

        var messageResponse = response.LastOrDefault()?.Content ?? string.Empty;

        return new AiResponse(messageResponse, 0, 0);
    }

    private string RenderPrompt(CopilotChatBot chatBot)
    {
        var currentDate = DateTimeOffset.UtcNow
            .ToOffset(TimeSpan.FromHours(chatBot.Timezone))
            .ToString("dddd, dd.MM.yyyy HH:mm");

        var prompt =
            $"""
             ### Instruction:
             {chatBot.PersonalityPrompt}

             ### Current Date and Time:
             {currentDate}
             """;

        return prompt;
    }

    private List<KernelPlugin> GetPlugins(DialogueManagerParameter parameter)
    {
        var kernelPlugins = PluginFactory
            .CreateFromCopilotPlugins(
                clientMessage: parameter.IncomingMessage,
                dialogue: parameter.Dialogue,
                plugins: parameter.Plugins,
                serviceProvider: _serviceProvider
            )
            .ToList();

        return kernelPlugins;
    }
}