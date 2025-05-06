namespace Copilot.Contracts.Responses;

public class WhatsAppAccountResponse
{
    public int IdInstance { get; set; }

    public string ApiTokenInstance { get; set; }
    
    public string PhoneNumber { get; set; }

    public Guid Id { get; set; }

    public Guid BotId { get; set; }

    public string BotTitle { get; set; }
}