namespace Test.Scopes.Domain.Models.Customers.CreateCustomer
{
    public record CreateCustomerRequest
    {
        public string Login { get; init; }

        public string Password { get; init; }

        public string Name { get; init; }

        public DateTime BirthDate { get; init; }

        public CreateContactRequest Contact { get; init; }

        public bool Active { get; init; } = true;

        public record CreateContactRequest
        {
            public string Email { get; init; }

            public string Phone { get; init; }
        }
    }
}
