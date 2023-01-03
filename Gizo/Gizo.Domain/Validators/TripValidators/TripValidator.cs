using FluentValidation;
using Gizo.Domain.Aggregates.TripAggregate;

namespace Gizo.Domain.Validators.TripValidators;

public class TripValidator : AbstractValidator<Trip>
{
    public TripValidator()
    {
        RuleFor(p => p.ChunkSize)
            .LessThan(524288000).WithMessage("Chunk Size must be Less Than 524288000 byte long")
            .GreaterThan(3).WithMessage("Chunk size must be at least 3145728 byte long");

        RuleFor(p => p.Score)
            .LessThan(10).WithMessage("Score must be Less Than 10 long");
    }
}
