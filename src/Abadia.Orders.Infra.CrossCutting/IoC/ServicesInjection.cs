using Abadia.Orders.Application.MessageProducer;
using Abadia.Orders.Shared.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class ServicesInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IProducerClient<UploadXlsFinishedIntegrationEvent>, ProducerClient<UploadXlsFinishedIntegrationEvent>>();
    }
}