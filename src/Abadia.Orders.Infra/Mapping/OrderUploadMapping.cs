using Abadia.Orders.Domain.OrderUploadAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abadia.Orders.Infra.Mapping;

public class OrderUploadMapping : IEntityTypeConfiguration<OrderUpload>
{
    public void Configure(EntityTypeBuilder<OrderUpload> builder)
    {
        builder.HasKey(o => o.Id);
    }
}