using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class SerilogInjection
{
    public static void AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
        .Enrich.WithCorrelationId()
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .WriteTo.Async(wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"))
            .WriteTo.Async(wt => wt.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DateTime.Now}.txt")))
            .WriteTo.MSSqlServer(configuration.GetConnectionString("OrderConnection"), new MSSqlServerSinkOptions()
            {
                TableName = "SerilogLogs",
                AutoCreateSqlTable = true,
            })
            .CreateLogger();
    }
}