namespace Abadia.Orders.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime UpdatedBy { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}
