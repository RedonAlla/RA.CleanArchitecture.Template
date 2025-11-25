using RA.Utilities.Core.Results;
using VsClArch.Template.Application.Models.TodoJsonPlaceholder;

namespace VsClArch.Template.Application.Abstractions;

/// <summary>
/// Defines the contract for a service that interacts with the Todo JSON Placeholder API.
/// </summary>
public interface ITodoJsonPlaceholder
{
    /// <summary>
    /// Asynchronously gets a random user from the Todo JSON Placeholder service.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TodoPlaceholderUser"/>.</returns>
    Task<Result<TodoPlaceholderUser>> GetRandomUserAsync(CancellationToken cancellationToken);
}
