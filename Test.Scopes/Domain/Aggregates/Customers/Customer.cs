using Test.Scopes.Abstractions.Domain.Aggregates;
using Test.Scopes.Domain.Models.Customers.CreateCustomer;
using Test.Scopes.Domain.Models.Customers.UpdateCustomer;
using Test.Scopes.Domain.ValueObjects.Contacts;
using Test.Scopes.Domain.ValueObjects.Credentials;

namespace Test.Scopes.Domain.Aggregates.Customers;

public class Customer : AggregateRoot<long>
{
    public string Name { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public Credential Credential { get; private set; }

    public Contact Contact { get; private set; }

    public void Register(CreateCustomerRequest request)
    {
        Name = request.Name;
        BirthDate = DateOnly.FromDateTime(request.BirthDate);
        Active = request.Active;

        Credential = new Credential(request.Login, request.Password);
        Contact = new Contact(request.Contact.Email, request.Contact.Phone);
        
        Validate();
    }

    public void Update(UpdateCustomerRequest request)
    {
        Name = request.Name ?? Name;
        BirthDate = request.BirthDate == default ? BirthDate : DateOnly.FromDateTime(request.BirthDate ?? default);

        if(request.Login != default || request.Password != default)
        {
            if (request.Password != default)
                Credential = new(request.Login ?? Credential.Login, request.Password);
            else
                Credential = new(request.Login ?? Credential.Login, Credential.Password);
        }

        if(request.Contact?.Email != default || request.Contact?.Phone != default)
            Contact = new(request.Contact?.Email ?? Contact.Email, request.Contact?.Phone ?? Contact.Phone);

        Validate();
    }

    public void Inactivate()
        => Active = false;

    public void Activate()
    {
        if (IsDeleted is true)
            return;

        Active = true;
    }

    public void Delete()
    {
        if (Active is true)
            return;

        IsDeleted = true;
    }

    protected override bool Validate()
        => OnValidate<CustomerValidator, Customer>();
}