namespace Test.Scopes.Domain.Interfaces.Handlers
{
    public interface IActivateCustomerHandler
    {
        Task Handle(long customerId, CancellationToken cancellationToken);
    }
}
