using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Domain.OrderUploadAggregate;
using Abadia.Orders.Shared.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Abadia.Orders.Application.Commands.Upload;

public class UploadXlsCommandHandler : IRequestHandler<UploadXlsCommand, ResponseContract>
{
    private readonly IOrderUploadRepository _orderUploadRepository;
    private readonly ILogger<UploadXlsCommandHandler> _logger;
    private readonly Domain.Messaging.IPublisher _publisherCommand;

    public UploadXlsCommandHandler(
        IOrderUploadRepository orderUploadRepository,
        ILogger<UploadXlsCommandHandler> logger,
        Domain.Messaging.IPublisher publisherCommand)
    {
        _orderUploadRepository = orderUploadRepository;
        _logger = logger;
        _publisherCommand = publisherCommand;
    }

    public async Task<ResponseContract> Handle(UploadXlsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing file: {name}", request.UploadFile.FileName);

        var response = new ResponseContract();
        response.SetResponse(code: HttpStatusCode.Created);

        var validationResult = request.Validate();

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("File: {name} is invalid", request.UploadFile.FileName);

            response.SetResponse(false, HttpStatusCode.BadRequest);

            return response;
        }

        var orderUpload = new OrderUpload(request.UploadFile.Name, request.UploadFile.ContentType, "");
        await _orderUploadRepository.CreateAsync(orderUpload);
        
        _logger.LogInformation("Created file: {name} with OrderUploadId: {id}", request.UploadFile.FileName, orderUpload.Id);

        SendMessage(orderUpload.Id);

        return response;
    }

    private void SendMessage(Guid orderUploadId)
    {
        _logger.LogInformation("Sending message to Rabbit, OrderUploadId: {id}", orderUploadId);

        var integrationEvent = new UploadXlsFinishedIntegrationEvent(orderUploadId);
        var message = new MessageContract<UploadXlsFinishedIntegrationEvent>(integrationEvent);

        _publisherCommand.Publish(message);
    }
}