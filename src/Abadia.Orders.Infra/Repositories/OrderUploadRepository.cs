using Abadia.Orders.Domain.OrderUploadAggregate;
using Abadia.Orders.Infra.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<OrderUpload?> GetOrderUploadAsync(Guid id)
    {
        return await _context.OrderUpload.FirstOrDefaultAsync(o => o.Id == id);
    }
}