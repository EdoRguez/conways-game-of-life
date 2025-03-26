namespace ConwaysGameOfLife.Contracts;

public class CreateBoardRequest
{
    public HashSet<Cell> Cells { get; set; } = new HashSet<Cell>();
}

public record Cell(int X, int Y);