using RA.Utilities.Core.Results;
using RA.Utilities.Data.Abstractions;
using RA.Utilities.Feature.Abstractions;
using VsClArch.Template.Domain.Entities;

namespace VsClArch.Template.Application.Todos.Get;

/// <summary>
/// Represents the handler for getting all Todo items.
/// </summary>
internal sealed class GetTodosHandler : IRequestHandler<GetTodosInput, List<GetTodosOutput>>
{
    private readonly IReadRepositoryBase<Todo> _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTodosHandler"/> class.
    /// </summary>
    /// <param name="repository">The read-only repository for Todo items.</param>
    public GetTodosHandler(IReadRepositoryBase<Todo> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Handles the query to get all Todo items.
    /// </summary>
    /// <param name="request">The input query. This is empty for getting all todos.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result{T}"/> with the list of Todo items.</returns>
    public async Task<Result<List<GetTodosOutput>>> HandleAsync(GetTodosInput request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Todo> data = await _repository.ListAsync(cancellationToken: cancellationToken);

        var todos = data.Select(t => new GetTodosOutput
        {
            Id = t.Id,
            UserId = t.UserId,
            Description = t.Description,
            DueDate = t.DueDate,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CompletedAt
        }).ToList();

        return todos;
    }
}
