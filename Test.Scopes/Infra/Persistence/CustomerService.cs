using Test.Scopes.Abstractions.Infra.Persistence;
using Test.Scopes.Domain.Aggregates.Customers;
using Test.Scopes.Domain.Interfaces.Persistence;
using Test.Scopes.Infra.Contexts;

namespace Test.Scopes.Infra.Persistence
{
    public class CustomerService : PersistenceService<Customer, long>, ICustomerService
    {
        public CustomerService(ICustomerRepository repository) 
            : base(repository) { }


    }
}
