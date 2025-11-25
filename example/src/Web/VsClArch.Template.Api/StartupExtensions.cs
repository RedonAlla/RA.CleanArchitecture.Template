using System.Reflection;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using RA.Utilities.Api.ExceptionHandlers;
using RA.Utilities.Api.Extensions;
using RA.Utilities.Authentication.JwtBearer.Extensions;
using VsClArch.Template.Api.Extensions;
using VsClArch.Template.Application;
using VsClArch.Template.Integration;
using VsClArch.Template.Persistence;
using VsClArch.Template.Persistence.Database;

namespace VsClArch.Template.Api;

/// <summary>
/// Provides extension methods for configuring services and the application's request pipeline.
/// </summary>
internal static class StartupExtensions
{
    /// <summary>
    /// Adds and configures various services to the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The application's <see cref="IConfiguration"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.RegisterOpenApi(configuration);

        services.AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails();

        services.AddEndpoints(Assembly.GetExecutingAssembly());

        services.AddJwtBearerAuthentication(configuration)
                .AddAuthorizationPolicies();

        services.AddApplicationServices()
                .AddIntegrationServices(configuration)
                .AddPersistence(configuration);

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

        if (app.Environment.IsDevelopment())
        {
            using IServiceScope scope = app.Services.CreateScope();
            using Task _ = scope.InitializeDatabaseAsync();
        }

        app.MapHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseMiddlewares()
            .UseHttpsRedirection()
            .UseExceptionHandler()
            .UseAuth();

        app.MapEndpoints();

        return app;
    }
}
