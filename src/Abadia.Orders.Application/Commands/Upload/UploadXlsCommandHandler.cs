using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Application.MessageProducer;
using Abadia.Orders.Domain.Interfaces;
using Abadia.Orders.Domain.OrderUploadAggregate;
using Abadia.Orders.Shared.IntegrationEvents;
using MediatR;

namespace Abadia.Orders.Application.Commands.Upload;

public class UploadXlsCommandHandler : IRequestHandler<UploadXlsCommand, ResponseContract>
{
    private readonly IOrderUploadRepository _orderUploadRepository;
    private readonly IProducerClient<UploadXlsFinishedIntegrationEvent> _producerClient;

    public UploadXlsCommandHandler(
        IOrderUploadRepository orderUploadRepository, 
        IProducerClient<UploadXlsFinishedIntegrationEvent> producerClient)
    {
        _orderUploadRepository = orderUploadRepository;
        _producerClient = producerClient;
    }

    public async Task<ResponseContract> Handle(UploadXlsCommand request, CancellationToken cancellationToken)
    {
        var response = new ResponseContract();
        response.SetResponse();

        //TODO VALIDATE
        var orderUpload = new OrderUpload(request.UploadFile.Name, request.UploadFile.ContentType, "");
        await _orderUploadRepository.CreateAsync(orderUpload);

        SendMessage(orderUpload.Id);

        return response;
    }

    private void SendMessage(Guid orderUploadId)
    {
        var integrationEvent = new UploadXlsFinishedIntegrationEvent(orderUploadId);
        var message = new MessageContract<UploadXlsFinishedIntegrationEvent>(integrationEvent);
        
        _producerClient.SendXlsMessage(message);
    }
}