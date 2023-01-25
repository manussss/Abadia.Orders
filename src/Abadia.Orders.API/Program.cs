using Abadia.Orders.API.ApiConfig;
using Abadia.Orders.Infra.CrossCutting.IoC;

namespace Abadia.Orders.API;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddMediator();
        builder.Services.AddApiConfiguration();
        builder.Services.AddSwagger();

        builder.Services.AddPublisher(builder.Configuration);

        var app = builder.Build();
        app.UseApiConfiguration(app.Environment);
        app.Run();
    }
}