using RA.Utilities.Core.Results;

namespace VsClArch.Template.Application.Todos.Get;

/// <summary>
/// Represents a handler for retrieving a list of Todo items.
/// Note: This class appears to be a backup or alternative version of GetTodosHandler.
/// </summary>
internal sealed class GetTodosHandlerBack()
{
    /// <summary>
    /// Handles the request to get all Todo items.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result{T}"/> with the list of Todo items.</returns>
    public Task<Result<List<GetTodosOutput>>> Handle(CancellationToken cancellationToken)
    {
        var todos = new List<GetTodosOutput>
        {
            new() {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Description = "Description",
                DueDate = DateTime.UtcNow.AddDays(20),
                Labels = ["FrontEnd", "Backend"],
                IsCompleted = false,
                CreatedAt = DateTime.Today
            }
        };
        return Task.FromResult<Result<List<GetTodosOutput>>>(todos);
    }
}
