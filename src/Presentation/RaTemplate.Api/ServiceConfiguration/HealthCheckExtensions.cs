using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RaTemplate.Api.ServiceConfiguration;

/// <summary>
/// Provides extension methods for registering and mapping default health checks.
/// </summary>
internal static class HealthCheckExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the default health checks to the application builder.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public IServiceCollection AddDefaultHealthChecks()
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return services;
        }
    }

    extension(WebApplication app)
    {
        /// <summary>
        /// Maps the default health endpoints for readiness and liveness checks.
        /// </summary>
        /// <returns>The <see cref="IApplicationBuilder"/> to allow for fluent chaining.</returns>
        public IApplicationBuilder MapHealthCheckEndpoints()
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });

            return app;
        }
    }
}
