#if UseAnyDatabase
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Data.EntityFramework.Extensions;
using RA.Utilities.Data.EntityFramework.Interceptors;
using RaTemplate.Application.Abstractions.Data;
using RaTemplate.Persistence.Database;

namespace RaTemplate.Persistence;

//TODO a more generic and reusable for multiple data bases

/// <summary>
/// Provides dependency injection for persistence services.
/// </summary>
public static class DependencyInjection
{
    private const string RaTemplateConnectionString = "RaTemplateConnectionString";

    /// <summary>
    /// Adds persistence services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration" /> containing application settings.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional services can be chained.</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration)
            .AddHealthChecks(configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BaseEntitySaveChangesInterceptor>();
        services.AddScoped<RaTemplateDbContextInitializer>();

        services.AddDbContext<RaTemplateDbContext>((provider, options) =>
        {
#if UseEfOracle
            options.UseOracle(GetConnectionString(configuration))
#endif
#if UseEfSqlServer
            options.UseSqlServer(GetConnectionString(configuration))
#endif
            options.AddInterceptors(provider.GetRequiredService<BaseEntitySaveChangesInterceptor>());

            IHostEnvironment env = provider.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                ILogger<RaTemplateDbContext> logger = provider.GetRequiredService<ILogger<RaTemplateDbContext>>();
                options.LogTo(
                        msg =>
                        {
                            if (logger.IsEnabled(LogLevel.Information))
                                logger.LogInformation("{Message}", msg);
                        },
                        [DbLoggerCategory.Database.Command.Name],
                        LogLevel.Information)
                    .EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IRaTemplateDbContext>(sp => sp.GetRequiredService<RaTemplateDbContext>());

        return services;
    }

    private static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
#if UseEfOracle
            .AddOracle(GetConnectionString(configuration));
#endif
#if UseEfSqlServer
            .AddSqlServer(GetConnectionString(configuration));
#endif
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(RaTemplateConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return connectionString;
    }
}
#endif
