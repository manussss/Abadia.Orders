namespace Abadia.Orders.Domain.OrderUploadAggregate;

public interface IOrderUploadRepository
{
    Task CreateAsync(OrderUpload orderUpload);
    Task<OrderUpload?> GetOrderUploadAsync(Guid id);
}