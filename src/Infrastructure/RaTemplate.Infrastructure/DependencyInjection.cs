using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//#if (UseIntegrations)
using RaTemplate.Integration;
//#endif
//#if (UseAnyDatabase)
using RaTemplate.Persistence;
//#endif

namespace RaTemplate.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure-layer services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="configuration">The application's <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> to allow for chaining of service registrations.</returns>
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            //#if (UseAnyDatabase)
            services.AddPersistence(configuration);
            //#endif
            //#if (UseIntegrations)
            services.AddIntegrationServices(configuration);
            //#endif

            return services;
        }
    }
}
