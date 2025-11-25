using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using RA.Utilities.Api.Abstractions;
using RA.Utilities.Api.Results;
using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;
using RA.Utilities.Api.Mapper;
using RA.Utilities.OpenApi.Utilities;
using VsClArch.Template.Application.Todos.Get;

namespace VsClArch.Template.Api.Endpoints.Todos;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("todos", async (
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) =>
            {
                Result<List<GetTodosOutput>> results =
                    await mediator.Send<GetTodosInput, List<GetTodosOutput>>(new GetTodosInput(), cancellationToken);

                return results.Match(SuccessResponse.Ok, ErrorResultResponse.Result);
            })
        .WithTags(Tags.Todos)
        .WithOrder(0)
        .WithName("GetTodoList")
        .WithDisplayName("Get List")
        .WithSummary("Get List")
        .WithDescription("Get list of TODOs.")
        .RequireAuthorization()
        .Produces<List<GetTodosOutput>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json)
        .Produces(StatusCodes.Status401Unauthorized, null, MediaTypeNames.Application.ProblemJson)
        .AddOpenApiOperationTransformer((operation, context, cancellationToken) =>
        {
            OpenApiOperationUtilities.AddResponseExample(operation, StatusCodes.Status200OK, "ListOfTodosExample", new OpenApiExample
            {
                Summary = "List of TODOs",
                Description = "This is an example of a todo list response.",
                Value = JsonSerializer.SerializeToNode(new List<GetTodosOutput>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        Description = "Todo Example 2",
                        DueDate = DateTime.UtcNow.AddDays(20),
                        Labels = ["FrontEnd", "Backend"],
                        IsCompleted = false,
                        CreatedAt = DateTime.Today
                    },
                    new() {
                        Id = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        Description = "Todo Example 1",
                        DueDate = DateTime.UtcNow.AddDays(20),
                        Labels = ["FrontEnd", "Backend"],
                        IsCompleted = false,
                        CreatedAt = DateTime.Today.AddDays(-2)
                    }
                })
            });

            OpenApiOperationUtilities.AddGeneralErrorResponse(operation);
            
            return Task.CompletedTask;
        });
    }
}
