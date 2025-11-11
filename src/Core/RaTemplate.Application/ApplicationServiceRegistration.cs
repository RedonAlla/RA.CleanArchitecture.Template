using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Feature.Extensions;

namespace RaTemplate.Application;

/// <summary>
/// Provides extension methods for registering application-layer services in the dependency injection container.
/// </summary>
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Adds the application services, including feature handlers and their decorators, to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        _ = services.AddMediator();

        return services;
    }
}
