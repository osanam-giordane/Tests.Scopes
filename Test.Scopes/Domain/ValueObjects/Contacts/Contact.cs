using Test.Scopes.Abstractions.Domain.ValueObjects;
using Test.Scopes.Domain.Aggregates.Customers;

namespace Test.Scopes.Domain.ValueObjects.Contacts;
public record Contact(string Email, string Phone) : ValueObject
{
    protected override bool Validate()
        => OnValidate<ContactValidator, Contact>();
}