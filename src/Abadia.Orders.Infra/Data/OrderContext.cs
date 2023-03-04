using Abadia.Orders.Domain.OrdersAggregate;
using Abadia.Orders.Domain.OrderUploadAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Abadia.Orders.Infra.Data;

public class OrderContext : IdentityDbContext<IdentityUser>
{
    public DbSet<OrderUpload> OrderUpload { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<Address> Address { get; set; }

    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);

        base.OnModelCreating(builder);
    }
}