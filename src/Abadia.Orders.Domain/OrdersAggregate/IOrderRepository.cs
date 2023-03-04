namespace Abadia.Orders.Domain.OrdersAggregate;

public interface IOrderRepository
{
    Task<int> CreateAsync(Order order);
}