using ConwaysGameOfLife.Core;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using ConwaysGameOfLife.Core.UseCases;
using Moq;
using Xunit;

namespace ConwaysGameOfLife.Tests.Core.UseCases;

public class CreateBoardHandlerTests
{
    private readonly Mock<IBoardRepository> _mockRepo = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly CreateBoardHandler _handler;

    public CreateBoardHandlerTests()
    {
        _handler = new CreateBoardHandler(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_InvalidCells_ReturnsFailure()
    {
        // Arrange
        var invalidCells = new HashSet<Cell> { new(-100, 1000) };

        // Act
        var result = await _handler.Handle(invalidCells);

        // Assert
        Assert.True(result.IsFailed);
        _mockRepo.Verify(r => r.CreateBoardAsync(It.IsAny<Board>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCells_CreatesAndSavesBoard()
    {
        // Arrange
        var validCells = new HashSet<Cell> { new(1, 1), new(1, 2) };

        // Act
        var result = await _handler.Handle(validCells);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepo.Verify(r => r.CreateBoardAsync(It.IsAny<Board>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}