using Abadia.Orders.Domain.OrdersAggregate;
using Abadia.Orders.Domain.OrderUploadAggregate;
using Abadia.Orders.Domain.Services;

namespace Abadia.Orders.Worker.XlsConverter.Application;

public class ConversorCommandHandler : IRequestHandler<ConversorCommand, bool>
{
    private readonly IOrderUploadRepository _orderUploadRepository;
    private readonly IFileConverter _fileConverter;
    private readonly IOrderRepository _orderRepository;

    public ConversorCommandHandler(IOrderUploadRepository orderUploadRepository, IFileConverter fileConverter, IOrderRepository orderRepository)
    {
        _orderUploadRepository = orderUploadRepository;
        _fileConverter = fileConverter;
        _orderRepository = orderRepository;
    }

    //TODO add logs
    public async Task<bool> Handle(ConversorCommand request, CancellationToken cancellationToken)
    {
        var orderUpload = await _orderUploadRepository.GetOrderUploadAsync(request.OrderUploadId);

        if (orderUpload == null)
            return false;

        var order = _fileConverter.ConvertFile(orderUpload.File);
        
        order.AddOrderUploadId(orderUpload.Id);
        order.CalculateTotalValue();

        await _orderRepository.CreateAsync(order);

        return true;
    }
}