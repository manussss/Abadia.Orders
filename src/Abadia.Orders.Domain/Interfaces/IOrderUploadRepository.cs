using Abadia.Orders.Domain.OrderUploadAggregate;

namespace Abadia.Orders.Domain.Interfaces;

public interface IOrderUploadRepository
{
    Task CreateAsync(OrderUpload orderUpload);
}