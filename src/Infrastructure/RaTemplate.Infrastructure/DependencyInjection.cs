using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RaTemplate.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure-layer services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The application's <see cref="IConfiguration"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow for chaining of service registrations.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
