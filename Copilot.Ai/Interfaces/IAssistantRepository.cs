using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;

namespace Copilot.Ai.Interfaces;

/// <summary>
/// Represents a repository for managing assistants.
/// </summary>
public interface IAssistantRepository
{
    /// <summary>
    /// Creates a new assistant with the specified instruction and GPT model.
    /// </summary>
    /// <param name="gptModel">The GPT model for the assistant.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created assistant.</returns>
    Task<IAgent> CreateAsync(string gptModel);

    /// <summary>
    /// Get the assistant asynchronously.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant.</param>
    /// <param name="plugins"></param>
    /// <returns>The agent associated with the assistant.</returns>
    Task<IAgent> GetAsync(string assistantId, List<KernelPlugin>? plugins = null);

    /// <summary>
    /// Delete the assistant asynchronously.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant.</param>
    Task DeleteAsync(string assistantId);

    /// <summary>
    /// Updates the assistant asynchronously.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant.</param>
    /// <param name="gptModel">The new GPT model for the assistant.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is an instance of <see cref="IAgent"/> representing the updated assistant.</returns>
    Task<IAgent> UpdateAsync(string assistantId, string gptModel);

    /// <summary>
    /// Creates a new agent thread asynchronously. </summary> <param name="assistantId">The ID of the assistant for which the thread is created.</param> <returns>
    /// Returns a Task of type IAgentThread representing the asynchronous operation.
    /// The result of the task contains the newly created agent thread. </returns>
    ///
    Task<IAgentThread> CreateThreadAsync(string assistantId);

    /// <summary>
    /// Retrieves the thread with the specified ID from the assistant with the specified ID.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant to retrieve the thread from.</param>
    /// <param name="threadId">The ID of the thread to retrieve.</param>
    /// <returns>The thread with the specified ID.</returns>
    Task<IAgentThread> GetThreadAsync(string assistantId, string threadId);

    /// <summary>
    /// Delete the assistant asynchronously.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant.</param>
    /// <param name="threadId">The ID of the thread.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DeleteThreadAsync(string assistantId, string threadId);
}