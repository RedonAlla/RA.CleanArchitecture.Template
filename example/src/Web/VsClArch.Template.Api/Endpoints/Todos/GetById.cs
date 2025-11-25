using Microsoft.AspNetCore.Mvc;
using RA.Utilities.Api.Abstractions;
using RA.Utilities.Core.Results;
using RA.Utilities.Feature.Abstractions;
using RA.Utilities.Api.Mapper;
using VsClArch.Template.Application.Todos.GetById;

namespace VsClArch.Template.Api.Endpoints.Todos;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("todos/{id:guid}", async (
            Guid id,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) =>
        {
            Result<GetTodosByIdOutput> results =
                await mediator.Send<GetTodosByIdInput, GetTodosByIdOutput>(new GetTodosByIdInput(id), cancellationToken);

            return results.Match(SuccessResponse.Ok, ErrorResultResponse.Result);
        })
        .RequireAuthorization()
        .WithTags(Tags.Todos)
        .WithOrder(1)
        .RequireAuthorization()
        .WithName("GetTodoById")
        .WithDisplayName("Get by Id")
        .WithSummary("Get by Id")
        .WithDescription("Get a TODO item by id.");
    }
}
