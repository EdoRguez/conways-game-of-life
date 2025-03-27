using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Infrastructure;
using ConwaysGameOfLife.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConwaysGameOfLife.Tests.Infrastructure.Repositories;

public class BoardRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly BoardRepository _repository;

    public BoardRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _context = new AppDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _repository = new BoardRepository(_context);
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }

    [Fact]
    public async Task GetBoardAsync_WhenExists_ReturnsBoard()
    {
        // Arrange
        var board = new Board { Id = Guid.NewGuid(), StateJson = "[]" };
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetBoardAsync(board.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(board.Id, result?.Id);
    }

    [Fact]
    public async Task CreateBoardAsync_AddsToContext()
    {
        // Arrange
        var board = new Board { Id = Guid.NewGuid(), StateJson = "[]" };

        // Act
        await _repository.CreateBoardAsync(board);
        await _context.SaveChangesAsync();

        // Assert
        Assert.Contains(board, _context.Boards);
    }
}