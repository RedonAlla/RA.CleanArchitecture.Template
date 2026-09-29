using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RA.Utilities.Data.EntityFramework.Interceptors;
using RaTemplate.Application.Abstractions.Data;
using RaTemplate.Persistence.Database;

namespace RaTemplate.Persistence;

/// <summary>
/// Provides dependency injection for persistence services.
/// </summary>
public static class DependencyInjection
{
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
        services.AddScoped(typeof(RaTemplateDbContextInitializer<>));

        //#if (UseEfSqlServer)
        services.AddDbContext<RaTemplateSqlServerDbContext>((provider, options) =>
        {
            options.UseSqlServer(GetConnectionString(configuration, "RaTemplateSqlServerConnectionString"));
            ConfigureOptions<RaTemplateSqlServerDbContext>(provider, options);
        });
        services.AddScoped<IRaTemplateSqlServerDbContext>(sp => sp.GetRequiredService<RaTemplateSqlServerDbContext>());
        //#endif
        //#if (UseEfOracle)
        services.AddDbContext<RaTemplateOracleDbContext>((provider, options) =>
        {
            options.UseOracle(GetConnectionString(configuration, "RaTemplateOracleConnectionString"));
            ConfigureOptions<RaTemplateOracleDbContext>(provider, options);
        });
        services.AddScoped<IRaTemplateOracleDbContext>(sp => sp.GetRequiredService<RaTemplateOracleDbContext>());
        //#endif
        //#if (UseEfPostgres)
        services.AddDbContext<RaTemplatePostgresDbContext>((provider, options) =>
        {
            options.UseNpgsql(GetConnectionString(configuration, "RaTemplatePostgresConnectionString"));
            ConfigureOptions<RaTemplatePostgresDbContext>(provider, options);
        });
        services.AddScoped<IRaTemplatePostgresDbContext>(sp => sp.GetRequiredService<RaTemplatePostgresDbContext>());
        //#endif
        //#if (UseEfSqlite)
        services.AddDbContext<RaTemplateSqliteDbContext>((provider, options) =>
        {
            options.UseSqlite(GetConnectionString(configuration, "RaTemplateSqliteConnectionString"));
            ConfigureOptions<RaTemplateSqliteDbContext>(provider, options);
        });
        services.AddScoped<IRaTemplateSqliteDbContext>(sp => sp.GetRequiredService<RaTemplateSqliteDbContext>());
        //#endif

        return services;
    }

    private static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        //#if (UseEfOracle)
        healthChecksBuilder.AddOracle(GetConnectionString(configuration, "RaTemplateOracleConnectionString"));
        //#endif
        //#if (UseEfSqlServer)
        healthChecksBuilder.AddSqlServer(GetConnectionString(configuration, "RaTemplateSqlServerConnectionString"));
        //#endif
        //#if (UseEfPostgres)
        healthChecksBuilder.AddNpgSql(GetConnectionString(configuration, "RaTemplatePostgresConnectionString"));
        //#endif
        //#if (UseEfSqlite)
        healthChecksBuilder.AddSqlite(GetConnectionString(configuration, "RaTemplateSqliteConnectionString"));
        //#endif
    }

    private static void ConfigureOptions<TContext>(IServiceProvider provider, DbContextOptionsBuilder options)
        where TContext : DbContext
    {
        options.AddInterceptors(provider.GetRequiredService<BaseEntitySaveChangesInterceptor>());

        IHostEnvironment env = provider.GetRequiredService<IHostEnvironment>();

        if (env.IsDevelopment())
        {
            ILogger<TContext> logger = provider.GetRequiredService<ILogger<TContext>>();
            options.LogTo(
                    msg =>
                    {
                        if (logger.IsEnabled(LogLevel.Information))
                        {
                            logger.LogInformation("{Message}", msg);
                        }
                    },
                    [DbLoggerCategory.Database.Command.Name],
                    LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }

    private static string GetConnectionString(IConfiguration configuration, string name)
    {
        string? connectionString = configuration.GetConnectionString(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return connectionString;
    }
}
