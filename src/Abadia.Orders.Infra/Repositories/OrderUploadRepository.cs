using Abadia.Orders.Domain.Interfaces;
using Abadia.Orders.Domain.OrderUploadAggregate;
using Abadia.Orders.Infra.Data;

namespace Abadia.Orders.Infra.Repositories;

public class OrderUploadRepository : IOrderUploadRepository
{
    private readonly OrderContext _context;

    public OrderUploadRepository(OrderContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(OrderUpload orderUpload)
    {
        _context.Add(orderUpload);
        await _context.SaveChangesAsync();
    }
}