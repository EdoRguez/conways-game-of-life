using ConwaysGameOfLife.Contracts;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.UseCases;
using MapsterMapper;
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

            if(res.IsFailed)
            {
                return Results.NotFound(res.Errors.First().Message);
            }

            return Results.Ok(new { LiveCells = res.Value });
        });

        group.MapPost("/", async ([FromServices] CreateBoardHandler handler, [FromServices] IMapper mapper, [FromBody] CreateBoardRequest request) =>
        {
            var model = mapper.Map<HashSet<Cell>>(request.Cells);
            var res = await handler.Handle(model);

            if(res.IsFailed)
            {
                return Results.BadRequest(res.Errors.First().Message);
            }

            return Results.Ok(new { Id = res.Value });
        });

        return group;
    }
}