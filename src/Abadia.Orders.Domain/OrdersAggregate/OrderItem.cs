using Abadia.Orders.Domain.Entities;

namespace Abadia.Orders.Domain.OrdersAggregate;

public class OrderItem : Entity
{
    public string Product { get; private set; }
    public decimal TotalValue { get; private set; }
    //public EnumPaymentType PaymentType { get; private set; }
    public Guid OrderId { get; private set; }
}
