using Microsoft.Extensions.Logging;
using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;
using RA.Utilities.Feature.Models;

namespace VsClArch.Template.Application.Todos.Create;


internal sealed class CreateTodoMessageDecorator : IPipelineBehavior<CreateTodoInput>
{
    // private readonly IFeatureHandler<CreateTodoInput> _innerHandler;
    private readonly ILogger<CreateTodoMessageDecorator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoMessageDecorator"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public CreateTodoMessageDecorator(ILogger<CreateTodoMessageDecorator> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result> HandleAsync(CreateTodoInput request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Before executing CreateTodoHandler...");
        Result result = await next();
        _logger.LogInformation("After executing CreateTodoHandler...");
        return result;
    }
}
