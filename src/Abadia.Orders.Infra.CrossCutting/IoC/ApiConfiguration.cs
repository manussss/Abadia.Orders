using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Abadia.Orders.Infra.CrossCutting.IoC;

public static class ApiConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Abadia Orders API",
            });
        });
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
        SwaggerBuilderExtensions.UseSwagger(app);
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
    }

    public static void AddApiConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning();

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment environment)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseApiVersioning();
        app.UseAuthorization();
        app.MapControllers();
    }
}