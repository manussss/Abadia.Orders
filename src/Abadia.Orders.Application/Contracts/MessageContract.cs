namespace Abadia.Orders.Application.Contracts;

public class MessageContract<T>
{
    public DateTime CreatedAt { get; set; }
    public T Data { get; set; }

    public MessageContract(T data)
    {
        CreatedAt = DateTime.Now;
        Data = data;
    }
}