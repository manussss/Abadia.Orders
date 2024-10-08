﻿using Abadia.Orders.Domain.Entities;
using Abadia.Orders.Domain.OrdersAggregate;

namespace Abadia.Orders.Domain.OrderUploadAggregate;

public class OrderUpload : Entity
{
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public string User { get; private set; }
    public byte[] File { get; private set; }
    public IReadOnlyCollection<Order> Orders => _orders;
    private readonly List<Order> _orders;

    public OrderUpload(
        string fileName,
        string contentType,
        string user,
        byte[] file)
    {
        FileName = fileName;
        ContentType = contentType;
        User = user;
        File = file;

        _orders = new();
    }
}