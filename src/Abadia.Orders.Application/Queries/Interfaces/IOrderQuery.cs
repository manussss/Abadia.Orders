using Abadia.Orders.Application.Queries.ViewModels;

namespace Abadia.Orders.Application.Queries.Interfaces;

public interface IOrderQuery
{
    Task<IEnumerable<OrdersDetailsViewModel>> GetOrdersByContractId(string contractId);
}
