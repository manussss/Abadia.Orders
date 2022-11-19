using Abadia.Orders.Application.Contracts;

namespace Abadia.Orders.Application.MessageProducer;

public interface IProducerClient<T>
{
    void SendXlsMessage(MessageContract<T> messageContract);
}
