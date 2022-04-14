namespace Test.Scopes.App.Models.Customers.Requests
{
    public record CreateCustomerRequest
    {
        public CreateCustomerRequest()
        {
            Contact = new();
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public CreateContactRequest Contact { get; set; }

        public bool Active { get; set; } = true;

        public record CreateContactRequest
        {
            public string Email { get; set; }

            public string Phone { get; set; }
        }
    }
}
