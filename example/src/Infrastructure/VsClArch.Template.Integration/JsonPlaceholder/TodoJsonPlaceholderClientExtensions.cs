using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Integrations.Extensions;
using VsClArch.Template.Application.Abstractions;

namespace VsClArch.Template.Integration.JsonPlaceholder;

/// <summary>
/// Provides extension methods for setting up the JSON Placeholder client in the dependency injection container.
/// </summary>
public static class TodoJsonPlaceholderClientExtensions
{
    /// <summary>
    /// Adds and configures the <see cref="ITodoJsonPlaceholder"/> and its dependencies to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configurationSection">The <see cref="IConfigurationSection"/> containing the settings for the JSON Placeholder API.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow for chaining of service registrations.</returns>
    public static IServiceCollection AddJsonPlaceholderClient(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services
            .AddHttpClientIntegration<ITodoJsonPlaceholder, TodoJsonPlaceholderClient, TodoJsonPlaceholderSettings>(configurationSection)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                // Only bypass in Development
                ServerCertificateCustomValidationCallback =
                    (msg, cert, chain, errors) => errors == System.Net.Security.SslPolicyErrors.None
            })
            .WithHttpLoggingHandler();

        return services;
    }
}
