using Abadia.Orders.Domain.OrdersAggregate;

namespace Abadia.Orders.Domain.Services;

public interface IFileConverter
{
    public Order Order { get; }
    Order ConvertFile(byte[] file);
}