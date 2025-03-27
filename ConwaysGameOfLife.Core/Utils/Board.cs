using ConwaysGameOfLife.Core.Entities;

namespace ConwaysGameOfLife.Core.Utils;

public static class Board
{
    public static HashSet<Cell> ComputeNextState(HashSet<Cell> liveCells)
    {
        var neighborCounts = new Dictionary<Cell, int>();
        foreach (var cell in liveCells)
        {
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                var neighbor = new Cell(cell.X + dx, cell.Y + dy);
                neighborCounts[neighbor] = neighborCounts.TryGetValue(neighbor, out var count) ? count + 1 : 1;
            }
        }

        var nextLiveCells = new HashSet<Cell>();
        foreach (var (cell, count) in neighborCounts)
        {
            bool isLive = liveCells.Contains(cell);
            if (isLive && (count == 2 || count == 3) || !isLive && count == 3)
                nextLiveCells.Add(cell);
        }

        return nextLiveCells;
    }
}