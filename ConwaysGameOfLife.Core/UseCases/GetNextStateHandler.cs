using System.Text.Json;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using FluentResults;

namespace ConwaysGameOfLife.Core.UseCases;

public class GetNextStateHandler
{
    private readonly IBoardRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public GetNextStateHandler(IBoardRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<HashSet<Cell>>> Handle(Guid boardId)
    {
        var board = await _repo.GetBoardAsync(boardId, true);
        if(board is null)
            return Result.Fail<HashSet<Cell>>("Board not found.");

        var currentState = JsonSerializer.Deserialize<HashSet<Cell>>(board.StateJson) ?? new HashSet<Cell>();
        var nextState = ComputeNextState(currentState);

        board.StateJson = JsonSerializer.Serialize(nextState);

        await _unitOfWork.SaveChangesAsync();
        return nextState;
    }

    public HashSet<Cell> ComputeNextState(HashSet<Cell> liveCells)
    {
        var neighborCounts = new Dictionary<Cell, int>();
        foreach (var cell in liveCells)
        {
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                var neighbor = new Cell(cell.X + dx, cell.Y + dy);
                neighborCounts[neighbor] = neighborCounts.TryGetValue(neighbor, out var count) ? count + 1 : 1;
            }
        }

        var nextLiveCells = new HashSet<Cell>();
        foreach (var (cell, count) in neighborCounts)
        {
            bool isLive = liveCells.Contains(cell);
            if (isLive && (count == 2 || count == 3) || !isLive && count == 3)
                nextLiveCells.Add(cell);
        }

        return nextLiveCells;
    }
}