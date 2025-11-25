using RA.Utilities.Core.Results;
using RA.Utilities.Data.Abstractions;
using RA.Utilities.Feature.Abstractions;
using VsClArch.Template.Domain.Entities;

namespace VsClArch.Template.Application.Todos.Create;

/// <summary>
/// Represents the handler for creating a new Todo item.
/// </summary>
internal sealed class CreateTodoHandler : IRequestHandler<CreateTodoInput>
{
    private readonly IWriteRepositoryBase<Todo> _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoHandler"/> class.
    /// </summary>
    /// <param name="repository">The write-only repository for Todo items.</param>
    public CreateTodoHandler(IWriteRepositoryBase<Todo> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Handles the command to create a new Todo item.
    /// </summary>
    /// <param name="request">The input command containing the details for the new Todo item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result"/> indicating the outcome.</returns>
    public async Task<Result> HandleAsync(CreateTodoInput request, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(new Todo
        {
            UserId = request.UserId,
            Description = request.Description,
            DueDate = request.DueDate,
            Labels = request.Labels,
            IsCompleted = request.IsCompleted
        }, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
