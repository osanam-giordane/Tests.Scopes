using FluentValidation;

namespace Test.Scopes.Domain.ValueObjects.Credentials;

public class CredentialValidator : AbstractValidator<Credential>
{
    public CredentialValidator()
    {
        RuleFor(credential => credential.Login)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5);

        RuleFor(credential => credential.Password)
            .NotNull()
            .NotEmpty();
    }
}
