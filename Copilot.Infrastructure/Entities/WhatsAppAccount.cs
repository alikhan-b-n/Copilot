namespace Copilot.Infrastructure.Entities;

public class WhatsAppAccount : BaseEntity
{
    public int IdInstance { get; set; }

    public string ApiTokenInstance { get; set; }

    public string PhoneNumber { get; set; }

    public Guid UserId { get; set; }

    public Guid BotId { get; set; }
}