using Microsoft.Extensions.Logging;
using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;
using RA.Utilities.Feature.Models;

namespace VsClArch.Template.Application.Todos.Edit;


internal sealed class EditTodoMessageDecorator : IPipelineBehavior<EditTodoInput, EditTodoOutput>
{
    private readonly ILogger<EditTodoMessageDecorator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditTodoMessageDecorator"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public EditTodoMessageDecorator(ILogger<EditTodoMessageDecorator> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<EditTodoOutput>> HandleAsync(EditTodoInput request, RequestHandlerDelegate<EditTodoOutput> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Before executing EditTodoMessageDecorator");
        _logger.LogInformation("Request: {@Request}", request);

        Result<EditTodoOutput> result = await next();

        _logger.LogInformation("After executing EditTodoMessageDecorator");
        _logger.LogInformation("Response: {@Response}", result.Value);

        return result;
    }
}
