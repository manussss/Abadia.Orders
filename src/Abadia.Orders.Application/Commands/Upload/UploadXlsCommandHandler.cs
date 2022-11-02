using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Domain.Interfaces;
using Abadia.Orders.Domain.OrderUploadAggregate;
using MediatR;

namespace Abadia.Orders.Application.Commands.Upload;

public class UploadXlsCommandHandler : IRequestHandler<UploadXlsCommand, ResponseContract>
{
    private readonly IOrderUploadRepository _orderUploadRepository;

    public UploadXlsCommandHandler(IOrderUploadRepository orderUploadRepository)
    {
        _orderUploadRepository = orderUploadRepository;
    }

    public async Task<ResponseContract> Handle(UploadXlsCommand request, CancellationToken cancellationToken)
    {
        var response = new ResponseContract();
        response.SetResponse();

        //VALIDATE

        await _orderUploadRepository.CreateAsync(new OrderUpload(request.UploadFile.Name, request.UploadFile.ContentType));

        //POST MESSAGE ON RABBIT

        return response;
    }
}