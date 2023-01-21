using MediatR;

namespace Abadia.Orders.Worker.XlsConverter.Application;

public class ConversorCommand : IRequest<bool>
{
    public Guid OrderUploadId { get; private set; }

    public ConversorCommand(Guid orderUploadId)
    {
        OrderUploadId = orderUploadId;
    }
}