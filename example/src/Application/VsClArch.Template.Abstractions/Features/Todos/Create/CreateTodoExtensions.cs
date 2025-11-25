using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Feature.Behaviors;
using RA.Utilities.Feature.Extensions;
using VsClArch.Template.Application.Todos.Create;

namespace VsClArch.Template.Application.Features.Todos.Create;

/// <summary>
/// Provides extension methods for registering the CreateTodo feature.
/// </summary>
public static class CreateTodoExtensions
{
    /// <summary>
    /// Adds the CreateTodo feature to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddCreateTodoFeature(this IServiceCollection services)
    {
        _ = services.AddMediator();

        _ = services
            .AddFeature<CreateTodoInput, CreateTodoHandler>()
            .AddDecoration<CreateTodoMessageDecorator>()
            .AddDecoration<LoggingBehavior<CreateTodoInput>>();
            
        return services;
    }
}
