using System.ComponentModel.DataAnnotations;

namespace ConwaysGameOfLife.Core.Entities;

public class Board
{
    public Guid Id { get; set; }
    public string StateJson { get; set; } = null!;
}