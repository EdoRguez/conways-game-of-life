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

        group.MapPost("/{id}/future/{steps}", async ([FromServices] GetFutureStateHandler handler, [FromServices] IMapper mapper,[FromRoute] string id, [FromRoute] int steps) =>
        {
            if(steps < 1)
                return Results.BadRequest("Steps must be >= 1.");

            var res = await handler.Handle(new Guid(id), steps);

            if(res.IsFailed)
            {
                return Results.BadRequest(res.Errors.First().Message);
            }

            return Results.Ok(new { LiveCells = res.Value });
        });

        group.MapPost("/{id}/final/{maxSteps}", async ([FromServices] GetFinalStateHandler handler, [FromServices] IMapper mapper,[FromRoute] string id, [FromRoute] int maxSteps) =>
        {
            if(maxSteps < 1)
                return Results.BadRequest("Max steps must be >= 1.");

            var res = await handler.Handle(new Guid(id), maxSteps);

            if(res.IsFailed)
            {
                return Results.BadRequest(res.Errors.First().Message);
            }

            return Results.Ok(new { LiveCells = res.Value });
        });

        return group;
    }
}