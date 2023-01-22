using Abadia.Orders.Domain.OrderUploadAggregate;

namespace Abadia.Orders.Worker.XlsConverter.Application;

public class ConversorCommandHandler : IRequestHandler<ConversorCommand, bool>
{
    private readonly IOrderUploadRepository _orderUploadRepository;

    public ConversorCommandHandler(IOrderUploadRepository orderUploadRepository)
    {
        _orderUploadRepository = orderUploadRepository;
    }

    public async Task<bool> Handle(ConversorCommand request, CancellationToken cancellationToken)
    {
        var orderUpload = await _orderUploadRepository.GetOrderUploadAsync(request.OrderUploadId);

        if (orderUpload == null)
            return false;

        //TODO Converter arquivo

        return true;
    }
}