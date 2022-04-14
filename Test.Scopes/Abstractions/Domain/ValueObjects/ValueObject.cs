using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Scopes.Abstractions.Domain.ValueObjects;

public abstract record ValueObject
{
    private ValidationResult ValidationResult { get; set; } = new();

    [NotMapped]
    public bool IsValid
        => Validate();

    [NotMapped]
    public IEnumerable<ValidationFailure> Errors
        => ValidationResult.Errors;

    protected bool OnValidate<TValidator, TValueObject>()
        where TValidator : AbstractValidator<TValueObject>, new()
        where TValueObject : ValueObject
    {
        ValidationResult = new TValidator().Validate(this as TValueObject);
        return ValidationResult.IsValid;
    }

    protected abstract bool Validate();
}