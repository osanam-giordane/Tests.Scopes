using Test.Scopes.Domain.Aggregates.Customers;

namespace Test.Scopes.Domain.Interfaces.Persistence
{
    public interface ICustomerService : IPersistenceService<Customer, long>
    {
    }
}
