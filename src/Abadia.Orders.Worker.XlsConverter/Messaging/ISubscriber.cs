using RabbitMQ.Client;

namespace Abadia.Orders.Worker.XlsConverter.Messaging;

public interface ISubscriber : IDisposable
{
    //void Subscribe<T>(T message) where T : class;
    public IModel Channel { get; }
}