using Abadia.Orders.Domain.Entities;
using Abadia.Orders.Domain.OrdersAggregate.Enums;

namespace Abadia.Orders.Domain.OrdersAggregate;

public class OrderItem : Entity
{
    public decimal TaxValue { get; private set; }
    public Guid OrderId { get; private set; }
    public Order? Order { get; private set; }
    public DateTime ReceiveDate { get; private set; }
    public DateTime DeliveryDate { get; private set; }
    public EnumOrderItemProduct Product { get; private set; }
    public string DocumentNumber { get; private set; }
    public Address? Address { get; private set; }
    public Guid AddressId { get; private set; } 

    public OrderItem(DateTime receiveDate, EnumOrderItemProduct product, string documentNumber)
    {
        ReceiveDate = receiveDate;
        Product = product;
        DocumentNumber = documentNumber;

        CalculateDeliveryDate();
        CalculateTaxValue();
    }

    protected OrderItem() { }

    public void AddOrderId(Guid id) => OrderId = id;
    public void AddAddress(Address address)
    {
        AddressId = address.Id;
        Address = address;
    }

    //TODO parametrizar as taxas
    //TODO calcular de acordo com a distância + tipo de produto (DomainService?)
    public void CalculateTaxValue()
    {
        var taxValue = Product == EnumOrderItemProduct.Car ? 1000 : 750;

        TaxValue = taxValue;
    }

    public void CalculateDeliveryDate()
    {
        var deliveryDate = Product == EnumOrderItemProduct.Car ? DateTime.Now.AddDays(7) : DateTime.Now.AddDays(3);

        DeliveryDate = deliveryDate;
    }
}
