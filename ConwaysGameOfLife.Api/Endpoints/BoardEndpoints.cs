using ConwaysGameOfLife.Contracts;
using ConwaysGameOfLife.Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ConwaysGameOfLife.Api.Endpoints;

public static class BoardEndpoints
{
    public static RouteGroupBuilder MapBoardEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/boards");

        group.MapGet("/{id}/next", async ([FromRoute] string id, [FromServices] GetNextStateHandler handler) =>
        {
            var res = await handler.Handle(new Guid(id));

            return Results.Ok();
        });

        group.MapPost("/", async ([FromServices] CreateBoardHandler handler, [FromBody] CreateBoardRequest request) =>
        {
            return Results.NoContent();
        });

        return group;
    }
}