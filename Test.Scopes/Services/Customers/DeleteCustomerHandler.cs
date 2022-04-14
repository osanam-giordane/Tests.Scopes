using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Interfaces.Persistence;

namespace Test.Scopes.Services.Customers
{
    public class DeleteCustomerHandler : IDeleteCustomerHandler
    {
        private readonly ICustomerService _service;

        public DeleteCustomerHandler(ICustomerService service)
            => _service = service;

        public async Task Handle(long customerId, CancellationToken cancellationToken)
        {
            var customer = await _service
                .GetOneAsync(customer => customer.Id == customerId, cancellationToken);

            if (customer == null)
                return;

            customer.Delete();

            await _service.SaveAsync(customer, cancellationToken);
        }
    }
}
