using System.Text.Json;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using FluentResults;

namespace ConwaysGameOfLife.Core.UseCases;

public class GetFinalStateHandler
{
    private readonly IBoardRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public GetFinalStateHandler(IBoardRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<HashSet<Cell>>> Handle(Guid boardId, int maxSteps)
    {
        var board = await _repo.GetBoardAsync(boardId, true);
        if(board is null)
            return Result.Fail<HashSet<Cell>>("Board not found.");

        for(int i = 0; i < maxSteps; i++)
        {
            var currentState = JsonSerializer.Deserialize<HashSet<Cell>>(board.StateJson) ?? new HashSet<Cell>();
            var nextState = Utils.Board.ComputeNextState(currentState);
            board.StateJson = JsonSerializer.Serialize(nextState);

            // Check if the board has stabilized
            if (currentState.SetEquals(nextState))
            {
                await _unitOfWork.SaveChangesAsync();
                return nextState;
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return Result.Fail<HashSet<Cell>>($"Board did not stabilize after {maxSteps} steps.");
    }
}