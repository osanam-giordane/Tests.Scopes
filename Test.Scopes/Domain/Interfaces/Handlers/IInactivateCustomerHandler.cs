namespace Test.Scopes.Domain.Interfaces.Handlers
{
    public interface IInactivateCustomerHandler
    {
        Task Handle(long customerId, CancellationToken cancellationToken);
    }
}
