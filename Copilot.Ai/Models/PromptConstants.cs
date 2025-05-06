namespace Copilot.Ai.Models;

public class PromptConstants
{
    public static readonly string PromptKey = "prompt";

    public static readonly string SystemPrompt =
        "Conversation with user. " +
        "Act as operator with own personality. " +
        "No long answers. \n" +
        "{{$" + PromptKey + "}}";
}