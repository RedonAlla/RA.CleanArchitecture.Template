using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Data.EntityFramework.Extensions;
using VsClArch.Template.Persistence.Database;
using VsClArch.Template.Persistence.Interceptors;

namespace VsClArch.Template.Persistence;

/// <summary>
/// Provides dependency injection for persistence services.
/// </summary>
public static class PersistenceDependencyInjection
{
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
            .AddPersistenceChecks()
            .AddReadRepositoryBase()
            .AddWriteRepositoryBase();

        //TODO this is for test or development
        // Not recommended for prod
        services.AddScoped<TodoDbContextInitializer>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<BaseEntitySaveChangesInterceptor>();

        services.AddDbContext<TodoDbContext>((provider, options) => options
            .UseInMemoryDatabase(connectionString ?? "TodoDatabase")
            .AddInterceptors(provider.GetRequiredService<BaseEntitySaveChangesInterceptor>()
        ));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<TodoDbContext>());

        return services;
    }

    private static IServiceCollection AddPersistenceChecks(this IServiceCollection services)
    {
        // This method is intentionally left without a DbContextCheck to avoid circular dependency issues during testing.
        services
            .AddHealthChecks();
            //.AddDbContextCheck<TodoDbContext>();
        return services;
    }
}
