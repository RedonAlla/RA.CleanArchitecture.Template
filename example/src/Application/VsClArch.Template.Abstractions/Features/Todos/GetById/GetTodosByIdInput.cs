using RA.Utilities.Feature.Abstractions;

namespace VsClArch.Template.Application.Todos.GetById;

/// <summary>
/// Represents the input for getting a Todo item by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the Todo item.</param>
public sealed record GetTodosByIdInput(Guid Id) : IRequest<GetTodosByIdOutput>;
