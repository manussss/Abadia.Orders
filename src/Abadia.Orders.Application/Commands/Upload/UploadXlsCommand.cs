using Abadia.Orders.Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Abadia.Orders.Application.Commands.Upload;

public class UploadXlsCommand : IRequest<ResponseContract>
{
    public IFormFile UploadFile { get; private set; }

    public UploadXlsCommand(IFormFile uploadFile)
    {
        UploadFile = uploadFile;
    }

    //TODO VALIDATIONRESULT
}