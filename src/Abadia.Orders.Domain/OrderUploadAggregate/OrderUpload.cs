using Abadia.Orders.Domain.Entities;
using Abadia.Orders.Domain.OrdersAggregate;

namespace Abadia.Orders.Domain.OrderUploadAggregate;

public class OrderUpload : Entity
{
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public string User { get; private set; }
    public Guid ClientId { get; private set; }
    public IReadOnlyCollection<Order> Orders => _orders;
    private readonly List<Order> _orders;

    public OrderUpload(
        string fileName, 
        string contentType,
        string user)
    {
        FileName = fileName;
        ContentType = contentType;
        User = user;
        ClientId = Guid.NewGuid();

        _orders = new();
    }

    //public void AddOrder(IEnumerable<Order> orders) => _orders.AddRange(orders);

    //public void AddOrder(Order order) => _orders.Add(order);

    //public void SetClientd(Guid clientId)
    //{
    //    ClientId = clientId;
    //}
}