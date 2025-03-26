using ConwaysGameOfLife.Core.Entities;

namespace ConwaysGameOfLife.Core.Repositories;

public interface IBoardRepository
{
    Task CreateBoardAsync(Board board);
    Task<Board?> GetBoardAsync(Guid boardId, bool trackChanges = false);
}