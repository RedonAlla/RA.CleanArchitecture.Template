using System;
using Microsoft.Extensions.Options;
using RA.Utilities.Core.Results;
using RA.Utilities.Integrations;
using VsClArch.Template.Application.Abstractions;
using VsClArch.Template.Application.Models.TodoJsonPlaceholder;
using VsClArch.Template.Integration.JsonPlaceholder.Users.GetUsers;

namespace VsClArch.Template.Integration.JsonPlaceholder;

/// <summary>
/// A client for interacting with the JSON Placeholder API, specifically for 'Todo' related operations.
/// </summary>
internal sealed class TodoJsonPlaceholderClient : BaseHttpClient, ITodoJsonPlaceholder
{
    private readonly TodoJsonPlaceholderActions _actions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoJsonPlaceholderClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance managed by the factory.</param>
    /// <param name="settings">The integration settings provided via the options pattern.</param>
    public TodoJsonPlaceholderClient(HttpClient httpClient, IOptions<TodoJsonPlaceholderSettings> settings) : base(httpClient, settings)
    {
        _actions = settings?.Value?.Actions ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<Result<TodoPlaceholderUser>> GetRandomUserAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<TodoPlaceholderUserResponse> users = await GetUsersAsync(cancellationToken);

        var mapper = new TodoPlaceholderUserMapper();

        return (users.Count > 0)
            ? mapper.Map(users[0])
            : new Exception("User not found"); //TODO Return not found exception.
    }
    
    private async Task<IReadOnlyList<TodoPlaceholderUserResponse>> GetUsersAsync(CancellationToken cancellationToken)
    {
        List<TodoPlaceholderUserResponse> users =
            await GetAsync<List<TodoPlaceholderUserResponse>>(action: _actions.GetUsers!, cancellationToken: cancellationToken);

        // Return the list of users, or an empty read-only array if the result is null.
        return users ?? (IReadOnlyList<TodoPlaceholderUserResponse>)[];
    }
}
