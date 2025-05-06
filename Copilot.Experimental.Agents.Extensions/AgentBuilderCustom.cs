using JetBrains.Annotations;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;
using Microsoft.SemanticKernel.Experimental.Agents.Extensions;
using Microsoft.SemanticKernel.Experimental.Agents.Internal;
using Microsoft.SemanticKernel.Experimental.Agents.Models;

namespace Copilot.Experimental.Agents.Extensions;

public class AgentBuilderCustom : AgentBuilder
{
    /// <summary>
    /// Retrieve an existing agent, by identifier.
    /// </summary>
    /// <param name="apiKey">A context for accessing OpenAI REST endpoint</param>
    /// <param name="agentId">The agent identifier</param>
    /// <param name="plugins">Plugins to initialize as agent tools</param>
    /// <param name="httpClient">Custom HTTP Client</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>An initialized <see cref="Agent"> instance.</see></returns>
    public static async Task<IAgent> GetAgentAsync(string apiKey,
        string agentId,
        IEnumerable<KernelPlugin>? plugins = null,
        HttpClient? httpClient = null,
        CancellationToken cancellationToken = default)
    {
        var restContext = new OpenAIRestContext(apiKey, () => httpClient);
        var resultModel = await restContext.GetAssistantModelAsync(agentId, cancellationToken).ConfigureAwait(false);

        return new Agent(resultModel, null, restContext, plugins);
    }
    
    /// <summary>
    /// Modify an existing agent, by identifier.
    /// </summary>
    /// <param name="apiKey">A context for accessing OpenAI REST endpoint</param>
    /// <param name="assistantId">The assistant identifier</param>
    /// <param name="model">The assistant chat model (required)</param>
    /// <param name="instructions">The assistant instructions (required)</param>
    /// <param name="name">The assistant name (optional)</param>
    /// <param name="description">The assistant description(optional)</param>
    /// <param name="httpClient">Custom <see cref="HttpClient"/> for HTTP requests.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>An initialized <see cref="Agent"> instance.</see></returns>
    public static async Task<IAgent> UpdateAgentAsync(
        string apiKey,
        string assistantId,
        string model,
        string instructions,
        string? name = null,
        string? description = null,
        [CanBeNull] HttpClient httpClient = null,
        CancellationToken cancellationToken = default)
    {
        var restContext = new OpenAIRestContext(apiKey, () => httpClient);
        var assistantModel = new AssistantModel
        {
            Model = model,
            Instructions = instructions,
            Name = name,
            Description = description
        };
        
        var resultModel =
            await restContext.UpdateAssistantModelAsync(assistantId, assistantModel, cancellationToken).ConfigureAwait(false) ??
            throw new KernelException($"Unexpected failure retrieving assistant: no result. ({assistantId})");

        return new Agent(resultModel, null, restContext);
    }

    public static async Task DeleteAgentAsync(
        string apiKey,
        string agentId,
        HttpClient httpClient,
        CancellationToken cancellationToken = default)
    {
        var agent = await GetAgentAsync(apiKey, agentId, httpClient: httpClient, cancellationToken: cancellationToken);
        await agent.DeleteAsync(cancellationToken);
    }
}