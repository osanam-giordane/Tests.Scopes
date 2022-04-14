namespace Test.Scopes.Domain.Interfaces.Handlers
{
    public interface IDeleteCustomerHandler
    {
        Task Handle(long customerId, CancellationToken cancellationToken);
    }
}
