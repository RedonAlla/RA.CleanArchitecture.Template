using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Dispatcher.Extensions;

namespace RaTemplate.Application;

/// <summary>
/// Provides extension methods for registering application-layer services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Use this registration only for applications with a single Presentation layer.
        /// </summary>
        /// <remarks>
        /// If multiple Presentation layers exist, register only the services required by each layer.
        /// </remarks>
        public IServiceCollection AddApplicationServices()
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            _ = services.AddMediator();
            return services;
        }
    }
}
