using System.Reflection;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using RA.Utilities.Api.ExceptionHandlers;
using RA.Utilities.Api.Extensions;
using RaTemplate.Api.Extensions;
using RaTemplate.Application;
using RaTemplate.Infrastructure;

namespace RaTemplate.Api;

internal static class StartupExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .RegisterOpenApi(configuration)
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddEndpoints(Assembly.GetExecutingAssembly());
#if UseAuthorization
        services.AddAuthorization(configuration);
#endif
        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        services.AddMiddlewares();

        return services;
    }

    /// <summary>
    /// Configures the application's request pipeline by adding various middlewares and endpoint mappings.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> to allow for fluent chaining.</returns>
    public static IApplicationBuilder UsePipelines(this WebApplication app)
    {
        if (!app.Environment.IsProduction())
        {
            app.UseOpenApi();
        }

        app.MapHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // if (app.Environment.IsDevelopment())
        // {
        //     using IServiceScope scope = app.Services.CreateScope();
        //     using Task _ = scope.InitializeDatabaseAsync();
        // }

        app.UseMiddlewares()
            .UseHttpsRedirection()
            .UseExceptionHandler();
#if UseAuthorization
        app.UseAuth(configuration);
#endif

        app.MapEndpoints();

        return app;
    }
}
