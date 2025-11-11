using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Data.EntityFramework.Extensions;
using RA.Utilities.Data.EntityFramework.Interceptors;
using RaTemplate.Persistence.Database;

namespace RaTemplate.Persistence;

/// <summary>
/// Provides dependency injection for persistence services.
/// </summary>
public static class PersistenceDependencyInjection
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
        services
            .AddDatabase(configuration)
            .AddPersistenceChecks(configuration)
            .AddReadRepositoryBase()
            .AddWriteRepositoryBase();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BaseEntitySaveChangesInterceptor>();

        services.AddDbContext<RaTemplateDbContext>((provider, options) => options
            .UseSqlServer(GetConnectionString(configuration))
            .AddInterceptors(provider.GetRequiredService<BaseEntitySaveChangesInterceptor>()
        ));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<RaTemplateDbContext>());

        return services;
    }

    private static IServiceCollection AddPersistenceChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddSqlServer(GetConnectionString(configuration));

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(RaTemplateConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return connectionString;
    }
}
