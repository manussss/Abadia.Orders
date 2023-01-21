namespace Abadia.Orders.Application.Contracts;

public class MessageContract<T>
{
    public T Data { get; set; }

    public MessageContract(T data)
    {
        Data = data;
    }
}