using Test.Scopes.Domain.Models.Customers.UpdateCustomer;

namespace Test.Scopes.Domain.Interfaces.Handlers
{
    public interface IUpdateCustomerHandler
    {
        Task Handle(long customerId, UpdateCustomerRequest request, CancellationToken cancellationToken);
    }
}
