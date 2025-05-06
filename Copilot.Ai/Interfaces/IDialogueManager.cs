using Copilot.Ai.Models;
using Copilot.Infrastructure.Entities;

namespace Copilot.Ai.Interfaces;

/// <summary>
/// Service gives answers as operator.
/// </summary>
public interface IDialogueManager
{
    /// <summary>
    /// Give human-like answers
    /// </summary>
    Task<AiResponse> GiveAnswer(DialogueManagerParameter parameter);
}

public record DialogueManagerParameter(
    string IncomingMessage,
    Dialogue Dialogue,
    List<Plugin> Plugins
);