namespace ConwaysGameOfLife.Contracts;

public class CreateBoardRequest
{
    public HashSet<CellRequest> Cells { get; set; } = new HashSet<CellRequest>();
}

public record CellRequest(int X, int Y);