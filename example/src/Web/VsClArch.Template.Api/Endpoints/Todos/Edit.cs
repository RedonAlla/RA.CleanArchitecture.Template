using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using RA.Utilities.Api.Abstractions;
using RA.Utilities.Api.Mapper;
using RA.Utilities.Api.Results;
using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;
using RA.Utilities.OpenApi.Utilities;
using VsClArch.Template.Api.Constants;
using VsClArch.Template.Api.Contracts.V1.Todos.Edit;
using VsClArch.Template.Application.Todos.Edit;

namespace VsClArch.Template.Api.Endpoints.Todos;

internal sealed class Edit : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("todos/{id:guid}", async (
            Guid id,
            [FromBody] EditTodoRequest request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) =>
            {
                var input = new EditTodoInput(request.Description, request.Labels, request.IsCompleted, request.DueDate);

                Result<EditTodoOutput> results =
                    await mediator.Send<EditTodoInput, EditTodoOutput>(input, cancellationToken);

                return results.Match(SuccessResponse.Ok, ErrorResultResponse.Result);
            })
        .RequireAuthorization(Polices.ManageTodo)
        .WithTags(Tags.Todos)
        .WithOrder(3)
        .WithName("EditTodo")
        .WithDisplayName("Edit")
        .WithSummary("Edit")
        .WithDescription("Edit a new Todo item.")
        .Accepts<EditTodoRequest>(MediaTypeNames.Application.Json)
        .Produces<EditTodoOutput>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<BadRequestResponse>(StatusCodes.Status400BadRequest, null, MediaTypeNames.Application.Json)
        .Produces(StatusCodes.Status401Unauthorized, null, MediaTypeNames.Application.ProblemJson)
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json)
        .AddOpenApiOperationTransformer((operation, context, cancellationToken) =>
        {
            OpenApiOperationUtilities.AddRequestExample(operation, "SimpleTodo", new OpenApiExample
            {
                Summary = "A simple todo item",
                Description = "This is an example of a minimal todo item, only providing the description.",
                Value = JsonSerializer.SerializeToNode(new EditTodoRequest
                {
                    Description = "Buy milk",
                    IsCompleted = false
                })
            });

            OpenApiOperationUtilities.AddRequestExample(operation, "TodoWithLabelsAndDueDate", new OpenApiExample
            {
                Summary = "A more complete todo item",
                Description = "This is an example of a todo item with labels and a due date.",
                Value = JsonSerializer.SerializeToNode(new EditTodoRequest
                {
                    Description = "Finish project report",
                    IsCompleted = false,
                    Labels = ["work", "urgent"],
                    DueDate = DateTime.Now.AddDays(3)
                })
            });

            OpenApiOperationUtilities.AddResponseExample(operation, StatusCodes.Status200OK, "SuccessfulEdit", new OpenApiExample
            {
                Summary = "A successfully edited todo item",
                Description = "This is an example of a todo item after a successful edit.",
                Value = JsonSerializer.SerializeToNode(new EditTodoResponse
                {
                    Description = "Finish project report and send for review",
                    IsCompleted = false,
                    Labels = ["work", "urgent", "review"],
                    DueDate = DateTime.Now.AddDays(2),
                    CreatedAt = DateTime.Now.AddDays(-5)
                })
            });

            OpenApiOperationUtilities.AddResponseExample(operation, StatusCodes.Status400BadRequest, "MinimumLengthValidator", new OpenApiExample
            {
                Summary = "MinimumLengthValidator",
                Description = "This is an example of a bad request due to a validation error.",
                Value = JsonSerializer.SerializeToNode(new BadRequestResponse([
                    new() {
                            PropertyName = "Description",
                            ErrorMessage = "The length of 'Description' must be at least 5 characters. You entered 3 characters.",
                            AttemptedValue = "Buy",
                            ErrorCode = "MinimumLengthValidator"
                        }
                    ]
                ))
            });

            OpenApiOperationUtilities.AddResponseExample(operation, StatusCodes.Status400BadRequest, "GreaterThanOrEqualValidator", new OpenApiExample
            {
                Summary = "GreaterThanOrEqualValidator",
                Description = "This is an example of a bad request due to a validation error.",
                Value = JsonSerializer.SerializeToNode(new BadRequestResponse([
                    new() {
                            PropertyName = "DueDate",
                            ErrorMessage = "'Due Date' must be greater than or equal to '17.9.2025 00:00:00'.",
                            AttemptedValue = "2025-09-15T20:29:19.36846+02:00",
                            ErrorCode = "GreaterThanOrEqualValidator"
                        }
                    ]
                ))
            });

            OpenApiOperationUtilities.AddGeneralErrorResponse(operation);

            return Task.CompletedTask;
        });
    }
}
