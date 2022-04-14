using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Interfaces.Persistence;

namespace Test.Scopes.Services.Customers
{
    public class InactivateCustomerHandler : IInactivateCustomerHandler
    {
        private readonly ICustomerService _service;

        public InactivateCustomerHandler(ICustomerService service)
            => _service = service;

        public async Task Handle(long customerId, CancellationToken cancellationToken)
        {
            var customer = await _service
                .GetOneAsync(customer => customer.Id == customerId, cancellationToken);

            if (customer == null)
                return;

            customer.Inactivate();

            await _service.SaveAsync(customer, cancellationToken);
        }
    }
}
