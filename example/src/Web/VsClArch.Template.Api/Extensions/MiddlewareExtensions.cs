using RA.Utilities.Api.Middlewares.Extensions;

namespace VsClArch.Template.Api.Extensions;

/// <summary>
/// Provides extension methods for registering and using custom middlewares.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds custom middlewares to the service collection.
    /// </summary>
    /// <remarks>
    /// This method currently registers the HTTP logging middleware and configures it to ignore
    /// specific paths like the root ("/") for the API documentation UI and the OpenAPI specification endpoint ("/openapi").
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        return services
            //.AddDefaultHeadersMiddleware()
            .AddHttpLoggingMiddleware(options =>
            {
                options.PathsToIgnore.Add("/openapi-ui");
                options.PathsToIgnore.Add("/openapi");
            });
    }

    /// <summary>
    /// Adds custom middlewares to the application's request pipeline.
    /// </summary>
    /// <remarks>
    /// This method currently adds the HTTP logging middleware to the pipeline.
    /// </remarks>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> to allow for fluent chaining.</returns>
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        return builder
            //.UseDefaultHeadersMiddleware()
            .UseHttpLoggingMiddleware();
    }
}
