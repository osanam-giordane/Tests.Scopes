namespace Test.Scopes.App.Models.Customers.Responses
{
    public record RecoverCustomersReponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool Active { get; set; }
    }
}
