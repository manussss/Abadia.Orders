using Abadia.Orders.Domain.OrdersAggregate;
using Abadia.Orders.Domain.Services;
using Abadia.Orders.Infra.CrossCutting.IoC;
using Abadia.Orders.Infra.Messaging;
using Abadia.Orders.Infra.Repositories;
using Abadia.Orders.Worker.XlsConverter;
using Abadia.Orders.Worker.XlsConverter.Application;
using Abadia.Orders.Worker.XlsConverter.Messaging;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddRepositories();
        services.AddHostedService<Worker>();
        services.AddDatabase(context.Configuration);
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient<ISubscriber, Subscriber>();
        services.AddSerilog(context.Configuration);
        services.AddTransient<IFileConverter, FileConverter>();
        services.AddTransient<IOrderRepository, OrderRepository>();
    })
    .Build();

await host.RunAsync();
