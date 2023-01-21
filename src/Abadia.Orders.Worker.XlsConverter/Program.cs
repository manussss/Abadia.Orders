using Abadia.Orders.Infra.CrossCutting.IoC;
using Abadia.Orders.Worker.XlsConverter;
using MediatR;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddRepositories();
        services.AddHostedService<Worker>();
        services.AddDatabase(context.Configuration);
        services.AddMediatR(Assembly.GetExecutingAssembly());
    })
    .Build();

await host.RunAsync();
