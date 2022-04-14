namespace Test.Scopes.Domain.Models.Customers.UpdateCustomer
{
    public record UpdateCustomerRequest
    {
        public string? Login { get; init; }

        public string? Password { get; init; }

        public string? Name { get; init; }

        public DateTime? BirthDate { get; init; }

        public UpdateContactRequest? Contact { get; init; }

        public record UpdateContactRequest
        {
            public string? Email { get; init; }

            public string? Phone { get; init; }
        }
    }
}
