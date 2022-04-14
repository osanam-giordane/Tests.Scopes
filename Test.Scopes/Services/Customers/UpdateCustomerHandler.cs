using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Interfaces.Persistence;
using Test.Scopes.Domain.Models.Customers.UpdateCustomer;

namespace Test.Scopes.Services.Customers
{
    public class UpdateCustomerHandler : IUpdateCustomerHandler
    {
        private readonly ICustomerService _service;

        public UpdateCustomerHandler(ICustomerService service)
            => _service = service;

        public async Task Handle(long customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _service
                .GetOneAsync(customer => customer.Id == customerId, cancellationToken);

            if (customer == null)
                return;

            customer.Update(request);

            await _service.SaveAsync(customer, cancellationToken);
        }
    }
}
