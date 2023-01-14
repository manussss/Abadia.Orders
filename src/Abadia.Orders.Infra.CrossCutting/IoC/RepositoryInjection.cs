using Abadia.Orders.Domain.OrderUploadAggregate;
using Abadia.Orders.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class RepositoryInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IOrderUploadRepository, OrderUploadRepository>();
    }
}