using ConwaysGameOfLife.Core;
using ConwaysGameOfLife.Core.Entities;
using ConwaysGameOfLife.Core.Repositories;
using ConwaysGameOfLife.Core.UseCases;
using Moq;
using Xunit;

namespace ConwaysGameOfLife.Tests.Core.UseCases;

public class GetNextStateHandlerTests
{
    private readonly Mock<IBoardRepository> _mockRepo = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly GetNextStateHandler _handler;

    public GetNextStateHandlerTests()
    {
        _handler = new GetNextStateHandler(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_NonExistingBoard_ReturnsFailure()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetBoardAsync(It.IsAny<Guid>(), true))
            .ReturnsAsync((Board)null!);

        // Act
        var result = await _handler.Handle(Guid.NewGuid());

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void ComputeNextState_BlockPattern_StaysStable()
    {
        // Arrange
        var block = new HashSet<Cell>
        {
            new(1, 1), new(1, 2),
            new(2, 1), new(2, 2)
        };

        // Act
        var result = _handler.ComputeNextState(block);

        // Assert
        Assert.Equal(block, result);
    }

    [Fact]
    public void ComputeNextState_BlinkerPattern_Oscillates()
    {
        // Arrange - Horizontal blinker
        var blinker = new HashSet<Cell>
        {
            new(1, 1), new(2, 1), new(3, 1)
        };

        // Act - After one iteration should be vertical
        var result = _handler.ComputeNextState(blinker);

        // Assert
        var expected = new HashSet<Cell>
        {
            new(2, 0), new(2, 1), new(2, 2)
        };
        Assert.Equal(expected, result);
    }
}