namespace Test.Scopes.App.Models.Customers.Requests
{
    public record UpdateCustomerRequest
    {
        public UpdateCustomerRequest()
        {
            Contact = new();
        }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public UpdateContactRequest? Contact { get; set; }

        public record UpdateContactRequest
        {
            public string? Email { get; set; }

            public string? Phone { get; set; }
        }
    }
}
