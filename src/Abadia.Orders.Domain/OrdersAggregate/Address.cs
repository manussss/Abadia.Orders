using Abadia.Orders.Domain.Entities;

namespace Abadia.Orders.Domain.OrdersAggregate;

public class Address : Entity
{
    public string Cep { get; private set; }
    public int Number { get; private set; }
    public string Complement { get; private set; }
    public string Street { get; private set; }
    public string? Reference { get; private set; }
    public OrderItem? OrderItem { get; private set; }

    public Address(string cep, int number, string complement, string street, string? reference)
    {
        Cep = cep;
        Number = number;
        Complement = complement;
        Street = street;
        Reference = reference;
    }
}