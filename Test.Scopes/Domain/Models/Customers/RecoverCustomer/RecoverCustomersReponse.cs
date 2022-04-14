namespace Test.Scopes.Domain.Models.Customers.RecoverCustomer
{
    public record RecoverCustomersReponse
    {
        public long Id { get; init; }

        public string Name { get; init; }

        public DateTime BirthDate { get; init; }

        public string Email { get; init; }

        public string Phone { get; init; }

        public bool Active { get; init; }
    }
}
