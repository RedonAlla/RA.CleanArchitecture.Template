using Microsoft.AspNetCore.Authorization;
using RA.Utilities.Authorization.Extensions;
using VsClArch.Template.Api.Constants;

namespace VsClArch.Template.Api.Authorization; 

internal sealed class TodoPermissionHandler : AuthorizationHandler<TodoPermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TodoPermissionRequirement requirement)
    {
        if(!context.User.HasScope(Scopes.TodoCreate) && !context.User.HasScope(Scopes.TodoEdit))
        {
            return Task.CompletedTask;
        }

        if (context.User.IsInRole(Roles.ToDoAdmin) || context.User.IsInRole(Roles.ToDoAuthor))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
