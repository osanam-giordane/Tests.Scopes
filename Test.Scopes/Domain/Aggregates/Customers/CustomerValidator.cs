using FluentValidation;
using Test.Scopes.Domain.ValueObjects.Contacts;
using Test.Scopes.Domain.ValueObjects.Credentials;

namespace Test.Scopes.Domain.Aggregates.Customers;
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Credential)
            .SetValidator(new CredentialValidator());

        RuleFor(customer => customer.Contact)
            .SetValidator(new ContactValidator());

        RuleFor(customer => customer.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(customer => customer.BirthDate)
            .NotEmpty()
            .NotNull()
            .InclusiveBetween(
                DateOnly.FromDateTime(DateTime.Now.AddYears(-95)), 
                DateOnly.FromDateTime(DateTime.Now.AddYears(-16).Date));
    }
}
