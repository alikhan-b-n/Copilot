namespace Copilot.Infrastructure.Entities.Altegio;

public class AltegioCompany : BaseEntity
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}