using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Abadia.Orders.Application.Commands.Upload;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class MediatorInjection
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UploadXlsCommand));
    }
}