using RA.Utilities.Feature.Abstractions;

namespace VsClArch.Template.Application.Todos.Edit;

/// <summary>
/// Represents the input for editing an existing Todo item.
/// </summary>
/// <param name="Description">The description of the Todo item.</param>
/// <param name="Labels">A list of labels for the Todo item.</param>
/// <param name="IsCompleted">A value indicating whether the Todo item is completed.</param>
/// <param name="DueDate">The optional due date for the Todo item.</param>
public sealed record EditTodoInput(
    string Description,
    List<string> Labels,
    bool IsCompleted,
    DateTime? DueDate
) : IRequest<EditTodoOutput>;
