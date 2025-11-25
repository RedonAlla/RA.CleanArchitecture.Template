using RA.Utilities.Feature.Abstractions;

namespace VsClArch.Template.Application.Todos.Get;

/// <summary>
/// Represents the input for the get all Todos query. This query does not require any parameters.
/// </summary>
public record GetTodosInput() : IRequest<List<GetTodosOutput>>;
