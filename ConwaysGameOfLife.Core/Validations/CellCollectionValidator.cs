using ConwaysGameOfLife.Core.Entities;
using FluentValidation;

namespace ConwaysGameOfLife.Core.Validations;

public class CellCollectionValidator : AbstractValidator<HashSet<Cell>>
{
    public CellCollectionValidator()
    {
        RuleForEach(cells => cells).SetValidator(new CellValidator());
        RuleFor(cells => cells)
            .NotEmpty().WithMessage("The collection must not be empty.")
            .Must(cells => cells.Count == cells.Distinct().Count())
            .WithMessage("The collection contains duplicate cells.");
    }
}