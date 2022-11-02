using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Abadia.Orders.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class DatabaseInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options => options.UseSqlServer(configuration.GetConnectionString("OrderConnection")));
    }
}