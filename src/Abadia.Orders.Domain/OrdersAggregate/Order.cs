﻿using Abadia.Orders.Domain.Entities;
using Abadia.Orders.Domain.OrdersAggregate.Enums;
using Abadia.Orders.Domain.OrderUploadAggregate;

namespace Abadia.Orders.Domain.OrdersAggregate;

public class Order : Entity
{
    public EnumOrderStatus OrderStatus { get; private set; }
    public string ContractId { get; private set; }
    public decimal TotalValue { get; private set; }
    public Guid OrderUploadId { get; private set; }
    public OrderUpload? OrderUpload { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    private readonly List<OrderItem> _orderItems;

    public Order(string contractId)
    {
        ContractId = contractId;

        OrderStatus = EnumOrderStatus.Created;

        _orderItems = new();
    }

    public void AddOrderUploadId(Guid orderUploadId) => OrderUploadId = orderUploadId;

    public void AddOrderItem(IEnumerable<OrderItem> orderItems)
    {
        _orderItems.AddRange(orderItems);
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        _orderItems.Add(orderItem);
    }

    public void CalculateTotalValue() => TotalValue = _orderItems.Sum(o => o.TaxValue);
}
