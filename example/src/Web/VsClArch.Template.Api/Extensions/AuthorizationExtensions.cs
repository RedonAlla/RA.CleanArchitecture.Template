using System;
using Microsoft.AspNetCore.Authorization;
using VsClArch.Template.Api.Authorization;
using VsClArch.Template.Api.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using RA.Utilities.Authorization.Extensions;

namespace VsClArch.Template.Api.Extensions;

internal static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAppUser();

        // A Simpler Alternative
        // If you have a simple scenario when you just need some roles, claims or scopes
        // you can pre-build the AuthorizationPolicy object and pass it to the AddPolicy method:

        // var adminGreetingsPolicy = new AuthorizationPolicyBuilder()
        //      .RequireRole(Roles.ToDoAdmin)
        //      .RequireClaim("scope", "greetings_api")
        //      .Build();

        // builder.Services.AddAuthorizationBuilder()
        //    .AddPolicy(Polices.CreateTodo, adminGreetingsPolicy);

        // This is less verbose but doesn't offer the same level of separation and reusability as the requirement/handler pattern.

        services.AddSingleton<IAuthorizationHandler, TodoPermissionHandler>();

        services.AddAuthorizationBuilder()
                .AddPolicy(Polices.ManageTodo, policy =>
                    policy.AddRequirements(new TodoPermissionRequirement()));

        return services;
    }
}
