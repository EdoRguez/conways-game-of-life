using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConwaysGameOfLife.Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;
    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Board?> GetBoardAsync(Guid boardId, bool trackChanges = false)
    {
        if(trackChanges)
            return await _context.Boards.SingleOrDefaultAsync(x => x.Id == boardId);

        return await _context.Boards.AsNoTracking().SingleOrDefaultAsync(x => x.Id == boardId);
    }

    public async Task CreateBoardAsync(Board board)
    {
        _context.Boards.Add(board);
        await Task.CompletedTask;
    }
}