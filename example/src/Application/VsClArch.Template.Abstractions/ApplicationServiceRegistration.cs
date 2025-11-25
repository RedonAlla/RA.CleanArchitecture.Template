using Microsoft.Extensions.DependencyInjection;
using RA.Utilities.Feature.Behaviors;
using RA.Utilities.Feature.Extensions;
using VsClArch.Template.Application.Features.Todos.Create;
using VsClArch.Template.Application.Features.Todos.Edit;
using VsClArch.Template.Application.Todos.Create;
using VsClArch.Template.Application.Todos.Edit;
using VsClArch.Template.Application.Todos.Get;
using VsClArch.Template.Application.Todos.GetById;

namespace VsClArch.Template.Application;

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
     
        _ = services
            .AddFeature<CreateTodoInput, CreateTodoHandler>()
            .AddDecoration<CreateTodoMessageDecorator>()
            .AddDecoration<LoggingBehavior<CreateTodoInput>>()
            .AddValidator<CreateTodoValidator>();

        _ = services
            .AddFeature<EditTodoInput, EditTodoOutput, EditTodoHandler>()
            .AddDecoration<EditTodoMessageDecorator>()
            .AddDecoration<LoggingBehavior<EditTodoInput, EditTodoOutput>>()
            .AddValidator<EditTodoValidator>();

        _ = services
            .AddFeature<GetTodosInput, List<GetTodosOutput>, GetTodosHandler>();

        _ = services
            .AddFeature<GetTodosByIdInput, GetTodosByIdOutput, GetTodosByIdHandler>();

        return services;
  }
}
