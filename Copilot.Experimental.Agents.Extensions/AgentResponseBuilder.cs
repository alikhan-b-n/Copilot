using System.Text;
using Microsoft.SemanticKernel.Experimental.Agents;

namespace Copilot.Experimental.Agents.Extensions;

/// <summary>
/// Represents a builder class for creating AgentResponse instances.
/// </summary>
public class AgentResponseBuilder
{
    private readonly StringBuilder _messageBuilder;
    private readonly StringBuilder _instructionsBuilder;
    
    public AgentResponseBuilder() : this(string.Empty, string.Empty)
    {
    }

    public AgentResponseBuilder(string message, string instructions)
    {
        _messageBuilder = new StringBuilder(message);
        _instructionsBuilder = new StringBuilder(instructions);
    }

    /// <summary>
    /// Appends a message to the existing message of the AgentResponseBuilder.
    /// </summary>
    /// <param name="message">The message to append.</param>
    /// <returns>The AgentResponseBuilder instance.</returns>
    public AgentResponseBuilder AppendMessage(string message)
    {
        _messageBuilder.AppendLine(message);

        return this;
    }

    /// <summary>
    /// Appends instructions to the existing instructions of the AgentResponseBuilder.
    /// </summary>
    /// <param name="instruction">The instruction to append.</param>
    /// <returns>The AgentResponseBuilder instance.</returns>
    public AgentResponseBuilder AppendInstructions(string instruction)
    {
        _instructionsBuilder.AppendLine(instruction);
        return this;
    }

    /// <summary>
    /// Builds an AgentResponse instance based on the current state of the AgentResponseBuilder.
    /// </summary>
    /// <param name="threadId">The thread ID to associate with the AgentResponse.</param>
    /// <returns>The built AgentResponse instance.</returns>
    public AgentResponse Build(string threadId)
    {
        var result = new AgentResponse
        {
            ThreadId = threadId,
            Message = _messageBuilder.ToString(),
            Instructions = _instructionsBuilder.ToString()
        };

        _messageBuilder.Clear();
        _instructionsBuilder.Clear();

        return result;
    }
}