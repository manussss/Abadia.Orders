namespace Abadia.Orders.Domain.Messaging;

public interface IPublisher : IDisposable
{
    void Publish<T>(T message) where T : class;
}