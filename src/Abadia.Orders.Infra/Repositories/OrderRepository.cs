using Abadia.Orders.Domain.OrdersAggregate;
using Abadia.Orders.Infra.Data;
using EFCore.BulkExtensions;

namespace Abadia.Orders.Infra.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderContext _context;
    private readonly BulkConfig _bulkConfig = new()
    {
        OnSaveChangesSetFK = false,
        PreserveInsertOrder = true,
        SetOutputIdentity = true,
        CalculateStats = true,
        UseTempDB = true,
    };

    public OrderRepository(OrderContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Order order)
    {
        int rowsAffected = 0;

        using (var transaction = _context.Database.BeginTransaction())
        {
            List<Order> orders = new() { order };

            await _context.BulkInsertAsync(orders, _bulkConfig);

            rowsAffected += AffectedRows(_bulkConfig.StatsInfo?.StatsNumberInserted);
            rowsAffected += await InsertAddress(order.OrderItems.Select(o => o.Address!).ToList());
            rowsAffected += await InserOrderItems(order.OrderItems.ToList());

            transaction.Commit();
        }

        return rowsAffected;
    }

    private async Task<int> InsertAddress(List<Address> addresses)
    {
        if (addresses.Count > 0)
        {
            await _context.BulkInsertAsync(addresses, _bulkConfig);
            
            return AffectedRows(_bulkConfig.StatsInfo?.StatsNumberInserted);
        }

        return 0;
    }

    private async Task<int> InserOrderItems(List<OrderItem> orderItems)
    {
        if (orderItems.Count > 0)
        {
            await _context.BulkInsertAsync(orderItems, _bulkConfig);

            return AffectedRows(_bulkConfig.StatsInfo?.StatsNumberInserted);
        }

        return 0;
    }

    private static int AffectedRows(int? value) => value ?? 0;
}