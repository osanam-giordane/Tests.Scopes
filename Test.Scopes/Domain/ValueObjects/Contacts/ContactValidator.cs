using FluentValidation;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Test.Scopes.Domain.ValueObjects.Contacts;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(contact => contact.Email)
            .NotEmpty()
            .NotNull()
            .Must(IsValidEmail);

        RuleFor(contact => contact.Phone)
            .NotEmpty()
            .NotNull()
            .Must(IsValidPhone);
    }

    private bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
            return false;
        
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhone(string phone)
    {
        phone = phone.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        return phone.Length < 15;
    }
}