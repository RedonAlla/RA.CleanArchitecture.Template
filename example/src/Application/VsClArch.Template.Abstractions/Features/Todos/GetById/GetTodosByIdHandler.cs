using Microsoft.Extensions.Logging;
using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;
using VsClArch.Template.Application.Abstractions;
using VsClArch.Template.Application.Models.TodoJsonPlaceholder;

namespace VsClArch.Template.Application.Todos.GetById;

/// <summary>
/// Represents the handler for getting a Todo item by its ID.
/// </summary>
internal sealed class GetTodosByIdHandler : IRequestHandler<GetTodosByIdInput, GetTodosByIdOutput>
{
    private readonly ILogger<GetTodosByIdHandler> _logger;
    private readonly ITodoJsonPlaceholder _jsonPlaceholder;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTodosByIdHandler"/> class.
    /// </summary>
    /// <param name="jsonPlaceholder">The JSON placeholder service.</param>
    /// <param name="logger">The logger.</param>
    public GetTodosByIdHandler(ITodoJsonPlaceholder jsonPlaceholder, ILogger<GetTodosByIdHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonPlaceholder = jsonPlaceholder ?? throw new ArgumentNullException(nameof(jsonPlaceholder));
    }

    /// <summary>
    /// Handles the query to get a Todo item by its ID.
    /// </summary>
    /// <param name="request">The input query containing the ID of the Todo item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result{T}"/> with the Todo item details.</returns>
    public async Task<Result<GetTodosByIdOutput>> HandleAsync(GetTodosByIdInput request, CancellationToken cancellationToken)
    {
        Result<TodoPlaceholderUser> authorResult = await _jsonPlaceholder.GetRandomUserAsync(cancellationToken);

        Author author = authorResult.Match(
            success: author => new Author(author.Name!, author.Username!, author.Email!),
            failure: ex =>
            {
                _logger.LogWarning("Failed to retrieve random user: {ErrorMessage}", ex.Message);
                return new Author();
            }
        );

        // TODO: Implement the logic for fetching a Todo item by ID from a data source.
        return new GetTodosByIdOutput
        {
            Id = request.Id,
            UserId = Guid.NewGuid(),
            Description = "Description",
            DueDate = DateTime.UtcNow.AddDays(20),
            Labels = ["FrontEnd", "Backend"],
            IsCompleted = false,
            CreatedAt = DateTime.Today,
            Author = author
        };
    }
}
