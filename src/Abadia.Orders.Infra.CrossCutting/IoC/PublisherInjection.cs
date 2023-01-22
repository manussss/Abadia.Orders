using Abadia.Orders.Domain.Messaging;
using Abadia.Orders.Infra.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class PublisherInjection
{
    public static void AddPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPublisher>(new Publisher(configuration));
    }
}