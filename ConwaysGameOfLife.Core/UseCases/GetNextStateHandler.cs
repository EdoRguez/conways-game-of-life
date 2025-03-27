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
        var nextState = Utils.Board.ComputeNextState(currentState);

        board.StateJson = JsonSerializer.Serialize(nextState);

        await _unitOfWork.SaveChangesAsync();
        return nextState;
    }
}