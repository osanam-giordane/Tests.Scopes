using Test.Scopes.Domain.Aggregates.Customers;

namespace Test.Scopes.Domain.Interfaces.Persistence
{
    public interface ICustomerRepository : IPersistenceRepository<Customer, long>
    {
    }
}
