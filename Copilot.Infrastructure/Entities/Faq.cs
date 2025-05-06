namespace Copilot.Infrastructure.Entities;

public class Faq : BaseEntity
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}