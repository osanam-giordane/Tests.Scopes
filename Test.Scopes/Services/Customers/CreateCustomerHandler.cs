using Test.Scopes.Domain.Aggregates.Customers;
using Test.Scopes.Domain.Interfaces;
using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Interfaces.Persistence;
using Test.Scopes.Domain.Models.Customers.CreateCustomer;

namespace Test.Scopes.Services.Customers;

public class CreateCustomerHandler : ICreateCustomerHandler
{
    private readonly ICustomerService _service;

    public CreateCustomerHandler(ICustomerService service)
        => _service = service;

    public Task<long> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer();
        customer.Register(request);

        return _service.SaveAsync(customer, cancellationToken);
    }
}