using Abadia.Orders.Domain.OrdersAggregate;
using Abadia.Orders.Domain.OrderUploadAggregate;
using Microsoft.EntityFrameworkCore;

namespace Abadia.Orders.Infra.Data;

public class OrderContext : DbContext
{
    public DbSet<OrderUpload> OrderUpload { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }

    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(entity => entity.Entity.GetType().GetProperty("CreatedAt") is not null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                entry.Property("CreatedBy").CurrentValue = "user test";
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                entry.Property("UpdatedBy").CurrentValue = "user test";
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}