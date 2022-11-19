using Abadia.Orders.Domain.Entities;

namespace Abadia.Orders.Domain.OrdersAggregate;

public class OrderItem : Entity
{
    public decimal Value { get; private set; }
    public Guid OrderId { get; private set; }
    public Order? Order { get; private set; }
    public decimal Taxes { get; private set; }
    public DateTime CreditDate { get; private set; }

    public OrderItem(
        decimal value, 
        Guid orderId, 
        decimal taxes, 
        DateTime creditDate)
    {
        Value = value;
        OrderId = orderId;
        Taxes = taxes;
        CreditDate = creditDate;
    }

    //TODO: Add taxes
}
