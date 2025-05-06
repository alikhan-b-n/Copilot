namespace Copilot.Contracts.Parameters;

public class NewMessageDialogueModelRequest
{
    public string ExternalIdentifier { get; set; }

    public string MessageContent { get; set; }

    public MessageRole Role { get; set; } = MessageRole.Client;
    
    public Guid ChatBotId { get; set; }
    
    public string? PhoneNumber { get; set; }
}

public enum MessageRole
{
    Client,
    Operator
}