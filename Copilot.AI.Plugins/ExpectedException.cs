namespace Copilot.AI.Plugins;

public class ExpectedException(string message, string instructions = "") : Exception(message)
{
    public string Instructions { get; } = instructions;
}