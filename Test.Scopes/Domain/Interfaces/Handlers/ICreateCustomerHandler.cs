using Test.Scopes.Domain.Models.Customers.CreateCustomer;

namespace Test.Scopes.Domain.Interfaces.Handlers;

public interface ICreateCustomerHandler
{
    Task<long> Handle(CreateCustomerRequest request, CancellationToken cancellationToken);
}