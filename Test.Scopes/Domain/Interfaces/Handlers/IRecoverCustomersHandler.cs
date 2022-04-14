using Test.Scopes.Domain.Models.Customers.RecoverCustomer;

namespace Test.Scopes.Domain.Interfaces.Handlers
{
    public interface IRecoverCustomersHandler
    {
        Task<IEnumerable<RecoverCustomersReponse>> Handle(CancellationToken cancellationToken);
    }
}
