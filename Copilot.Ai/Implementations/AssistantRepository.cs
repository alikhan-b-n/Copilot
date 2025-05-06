using System.Net;
using Copilot.Ai.Interfaces;
using Copilot.Ai.Models;
using Copilot.Ai.Utils.Options;
using Copilot.Experimental.Agents.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;

namespace Copilot.Ai.Implementations;

/// <inheritdoc />
public class AssistantRepository : IAssistantRepository
{
    private readonly IOptions<OpenAiModelOptions> _openAiModelOptions;
    private readonly HttpClient _httpClient;

    public AssistantRepository(IOptions<OpenAiModelOptions> openAiModelOptions)
    {
        _openAiModelOptions = openAiModelOptions;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestVersion = HttpVersion.Version20;
        _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
        _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
    }

    /// <inheritdoc />
    public async Task<IAgent> CreateAsync(string gptModel)
    {
        var assistantBuilder = new AgentBuilder()
            .WithInstructions(PromptConstants.SystemPrompt)
            .WithName("Assistant")
            .WithOpenAIChatCompletion(gptModel, _openAiModelOptions.Value.ApiKey)
            .WithHttpClient(_httpClient);

        return await assistantBuilder.BuildAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string assistantId)
    {
        await AgentBuilderCustom.DeleteAgentAsync(
            _openAiModelOptions.Value.ApiKey,
            assistantId,
            httpClient: _httpClient);
    }

    /// <inheritdoc />
    public async Task<IAgent> GetAsync(string assistantId, List<KernelPlugin>? plugins = null)
    {
        var assistant = await AgentBuilderCustom.GetAgentAsync(
            _openAiModelOptions.Value.ApiKey,
            assistantId,
            httpClient: _httpClient,
            plugins: plugins);

        return assistant;
    }

    /// <inheritdoc />
    public async Task<IAgent> UpdateAsync(
        string assistantId, string gptModel)
    {
        var assistant = await AgentBuilderCustom.UpdateAgentAsync(
            apiKey: _openAiModelOptions.Value.ApiKey,
            assistantId: assistantId,
            model: gptModel,
            instructions: PromptConstants.SystemPrompt,
            httpClient: _httpClient
        );

        return assistant;
    }

    /// <inheritdoc />
    public async Task<IAgentThread> CreateThreadAsync(string assistantId)
    {
        var assistant = await GetAsync(assistantId);
        var thread = await assistant.NewThreadAsync();
        return thread;
    }

    /// <inheritdoc />
    public async Task<IAgentThread> GetThreadAsync(string assistantId, string threadId)
    {
        var assistant = await GetAsync(assistantId);
        var thread = await assistant.GetThreadAsync(threadId);
        return thread;
    }
    
    /// <inheritdoc />
    public async Task DeleteThreadAsync(string assistantId, string threadId)
    {
        var assistant = await GetAsync(assistantId);
        var thread = await assistant.GetThreadAsync(threadId);
        await thread.DeleteAsync();
    }
}