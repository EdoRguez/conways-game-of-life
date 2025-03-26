using ConwaysGameOfLife.Core.Entities;
using FluentValidation;

namespace ConwaysGameOfLife.Core.Validations;

public class CellValidator : AbstractValidator<Cell>
{
    public CellValidator()
    {
        RuleFor(cell => cell.X).GreaterThanOrEqualTo(0);
        RuleFor(cell => cell.Y).GreaterThanOrEqualTo(0);
    }
}