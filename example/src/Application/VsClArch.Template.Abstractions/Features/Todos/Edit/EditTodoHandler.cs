using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;

namespace VsClArch.Template.Application.Todos.Edit;

/// <summary>
/// Represents the handler for editing a Todo item.
/// </summary>
internal sealed class EditTodoHandler : IRequestHandler<EditTodoInput, EditTodoOutput>
{
    public Task<Result<EditTodoOutput>> HandleAsync(EditTodoInput request, CancellationToken cancellationToken)
    {
        // TODO: Implement the logic for editing a Todo item.
        return Task.FromResult<Result<EditTodoOutput>>(new EditTodoOutput
        {
            UserId = Guid.NewGuid(),
            Description = request.Description,
            DueDate = request.DueDate,
            Labels = request.Labels,
            IsCompleted = request.IsCompleted,
            CreatedAt = DateTime.Today
        });
    }
}
