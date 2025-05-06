using Microsoft.SemanticKernel.Experimental.Agents;
using Microsoft.SemanticKernel.Experimental.Agents.Extensions;
using Microsoft.SemanticKernel.Experimental.Agents.Internal;
using Microsoft.SemanticKernel.Experimental.Agents.Models;

namespace Copilot.Experimental.Agents.Extensions;

public static partial class OpenAIRestExtensionsCustom
{
    /// <summary>
    /// Modify an existing assistant, by identifier.
    /// </summary>
    /// <param name="context">A context for accessing OpenAI REST endpoint</param>
    /// <param name="assistantId">The assistant identifier</param>
    /// <param name="model">The assistant definition</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>An assistant definition</returns>
    internal static Task<AssistantModel> UpdateAssistantModelAsync(
        this OpenAIRestContext context,
        string assistantId,
        AssistantModel model,
        CancellationToken cancellationToken = default)
    {
        var payload =
            new
            {
                model = model.Model,
                instructions = model.Instructions,
                tools = model.Tools
            };
        string requestUrl = GetAssistantUrl(assistantId);
        
        return
            context.ExecutePostAsync<AssistantModel>(
                requestUrl,
                payload,
                cancellationToken);
    }
    
    internal static string GetAssistantUrl(string assistantId)
    {
        return $"{OpenAIRestExtensions.BaseAssistantUrl}/{assistantId}";
    }

}