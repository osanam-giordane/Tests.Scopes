using Test.Scopes.Abstractions.Infra.Persistence;
using Test.Scopes.Domain.Aggregates.Customers;
using Test.Scopes.Domain.Interfaces.Persistence;
using Test.Scopes.Infra.Contexts;

namespace Test.Scopes.Infra.Persistence;

public class CustomerRepository : PersistenceRepository<Customer, long>, ICustomerRepository
{
    public CustomerRepository(PersistenceDbContext dbContext)
        : base(dbContext) { }
}