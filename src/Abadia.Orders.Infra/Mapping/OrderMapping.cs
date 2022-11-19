using Abadia.Orders.Domain.OrdersAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abadia.Orders.Infra.Mapping;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasOne(o => o.OrderUpload)
            .WithMany(ou => ou.Orders)
            .HasForeignKey(o => o.OrderUploadId);
    }
}