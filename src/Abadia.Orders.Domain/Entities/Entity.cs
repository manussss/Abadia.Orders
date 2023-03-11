namespace Abadia.Orders.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}
