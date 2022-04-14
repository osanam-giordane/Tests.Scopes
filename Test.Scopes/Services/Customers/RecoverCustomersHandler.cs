using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Interfaces.Persistence;
using Test.Scopes.Domain.Models.Customers.RecoverCustomer;

namespace Test.Scopes.Services.Customers
{
    public class RecoverCustomersHandler : IRecoverCustomersHandler
    {
        private readonly ICustomerService _service;

        public RecoverCustomersHandler(ICustomerService service)
            => _service = service;

        public async Task<IEnumerable<RecoverCustomersReponse>> Handle(CancellationToken cancellationToken)
        {
            var customers = await _service.GetAsync(customer => true, cancellationToken);

            return customers.Select(customer => new RecoverCustomersReponse()
            {
                Id = customer.Id,
                Name = customer.Name,
                BirthDate = customer.BirthDate.ToDateTime(TimeOnly.MinValue),
                Email = customer.Contact.Email,
                Phone = customer.Contact.Phone,
                Active = customer.Active
            });
        }
    }
}
