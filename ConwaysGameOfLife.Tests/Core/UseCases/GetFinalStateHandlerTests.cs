using System.Text.Json;
using ConwaysGameOfLife.Core;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using ConwaysGameOfLife.Core.UseCases;
using Moq;
using Xunit;

namespace ConwaysGameOfLife.Tests.Core.UseCases;

public class GetFinalStateHandlerTests
{
    private readonly Mock<IBoardRepository> _mockRepo = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly GetFinalStateHandler _handler;

    public GetFinalStateHandlerTests()
    {
        _handler = new GetFinalStateHandler(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_NonExistingBoard_ReturnsFailure()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetBoardAsync(It.IsAny<Guid>(), true))
            .ReturnsAsync((Board)null!);

        // Act
        var result = await _handler.Handle(Guid.NewGuid(), 3);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task Handle_BlinkerPattern_ReturnsCorrectStateAfterSteps()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var initialCells = new HashSet<Cell> { new(1, 1), new(2, 1), new(3, 1) };
        var board = new Board{ Id = boardId, StateJson = JsonSerializer.Serialize(initialCells) };

        _mockRepo.Setup(r => r.GetBoardAsync(boardId, true))
            .ReturnsAsync(board);

        // Blinker should return to original state after 2 steps
        var expectedState = initialCells;

        // Act
        var result = await _handler.Handle(boardId, 205);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedState, result.Value);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(default));
    }

    [Fact]
    public async Task Handle_ZeroSteps_ReturnsCurrentState()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var initialCells = new HashSet<Cell> { new(0, 0) };
        var board = new Board{ Id = boardId, StateJson = JsonSerializer.Serialize(initialCells) };

        _mockRepo.Setup(r => r.GetBoardAsync(boardId, true))
            .ReturnsAsync(board);

        // Act
        var result = await _handler.Handle(boardId, 0);

        // Assert
        Assert.False(result.IsSuccess);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(default));
    }
}