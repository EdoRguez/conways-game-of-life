using System.Text.Json;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using FluentResults;

namespace ConwaysGameOfLife.Core.UseCases;

public class GetFutureStateHandler
{
    private readonly IBoardRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public GetFutureStateHandler(IBoardRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<HashSet<Cell>>> Handle(Guid boardId, int steps)
    {
        var board = await _repo.GetBoardAsync(boardId, true);
        if(board is null)
            return Result.Fail<HashSet<Cell>>("Board not found.");

        for (int i = 0; i < steps; i++)
        {
            var currentState = JsonSerializer.Deserialize<HashSet<Cell>>(board.StateJson) ?? new HashSet<Cell>();
            var nextState = Utils.Board.ComputeNextState(currentState);
            board.StateJson = JsonSerializer.Serialize(nextState);
        }

        await _unitOfWork.SaveChangesAsync();

        var finalState = JsonSerializer.Deserialize<HashSet<Cell>>(board.StateJson) ?? new HashSet<Cell>();

        return finalState;
    }
}