using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Scopes.Abstractions.Domain.Aggregates;
public abstract class AggregateRoot<TId> where TId : struct
{
    [NotMapped]
    private ValidationResult _validationResult = new();

    [NotMapped]
    public IEnumerable<ValidationFailure> Errors
        => _validationResult.Errors;

    [NotMapped]
    public bool IsValid
        => Validate();

    public TId Id { get; init; }

    public bool IsDeleted { get; protected set; } = false;

    public bool Active { get; protected set; }

    public DateTime CreateAt { get; private set; } = DateTime.Now;

    protected bool OnValidate<TValidator, TAggregate>()
        where TValidator : AbstractValidator<TAggregate>, new()
        where TAggregate : AggregateRoot<TId>
    {
        _validationResult = new TValidator().Validate(this as TAggregate);
        return _validationResult.IsValid;
    }

    protected abstract bool Validate();
}
