using System.Text.Json;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using ConwaysGameOfLife.Core.Validations;
using FluentResults;

namespace ConwaysGameOfLife.Core.UseCases;

public class CreateBoardHandler
{
    private readonly IBoardRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBoardHandler(IBoardRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(HashSet<Cell> liveCells)
    {
        var validator = new CellCollectionValidator();
        var result = await validator.ValidateAsync(liveCells);

        if (!result.IsValid)
        {
            return Result.Fail<Guid>(result.Errors.Select(e => e.ErrorMessage).First());
        }

        var board = new Board
        {
            Id = Guid.NewGuid(),
            StateJson = JsonSerializer.Serialize(liveCells)
        };

        await _repo.CreateBoardAsync(board);
        await _unitOfWork.SaveChangesAsync();

        return board.Id;
    }
}