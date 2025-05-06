namespace Copilot.Infrastructure.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.Now;
}