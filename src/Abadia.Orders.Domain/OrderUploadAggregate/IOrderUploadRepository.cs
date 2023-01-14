namespace Abadia.Orders.Domain.OrderUploadAggregate;

public interface IOrderUploadRepository
{
    Task CreateAsync(OrderUpload orderUpload);
}