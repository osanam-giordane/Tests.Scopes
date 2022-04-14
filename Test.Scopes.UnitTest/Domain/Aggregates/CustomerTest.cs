using Bogus;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Scopes.Domain.Aggregates.Customers;
using Test.Scopes.Domain.Models.Customers.CreateCustomer;
using Test.Scopes.Domain.Models.Customers.UpdateCustomer;
using Test.Scopes.UnitTest.Extensions;
using Xunit;

namespace Test.Scopes.UnitTest.Domain.Aggregates
{
    public class CustomerTest
    {
        [Fact]
        public void Register_Customer()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .Then(tuple => tuple.customer.Register(tuple.request))
                .And(tuple => tuple.customer)
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Id.Should().Be(0));

        [Fact]
        public void Register_Customer_InvalidBirthDate()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(requestFake => requestFake
                    .RuleFor(customer => customer.BirthDate, Faker => Faker.Date.Recent()))
                .When(requestFake => requestFake.Generate())
                .When(request => (request, customer: new Customer()))
                .Then(tuple => tuple.customer.Register(tuple.request))
                .And(tuple => tuple.customer)
                .Then(customer => customer.IsValid.Should().BeFalse());

        [Fact]
        public void Inactivate_Customer()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .Then(customer => customer.Inactivate())
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Active.Should().BeFalse());

        [Fact]
        public void Delete_ActiveCustomer()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .Then(customer => customer.Delete())
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Active.Should().BeTrue())
                .Then(customer => customer.IsDeleted.Should().BeFalse());

        [Fact]
        public void Delete_InactiveCustomer()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => fakeRequest
                    .RuleFor(customer => customer.Active, fake => false))
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .Then(customer => customer.Delete())
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Active.Should().BeFalse())
                .Then(customer => customer.IsDeleted.Should().BeTrue());

        [Fact]
        public void Activate_Customer_Deleted()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => fakeRequest
                    .RuleFor(customer => customer.Active, fake => false))
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .When(customer => customer.Delete())
                .Then(customer => customer.Activate())
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Active.Should().BeFalse())
                .Then(customer => customer.IsDeleted.Should().BeTrue());

        [Fact]
        public void Activate_Customer_Inactivated()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => fakeRequest
                    .RuleFor(customer => customer.Active, fake => false))
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .Then(customer => customer.Activate())
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Active.Should().BeTrue());

        [Fact]
        public void Inactivate_Customer_Deleted()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => fakeRequest
                    .RuleFor(customer => customer.Active, fake => false))
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .When(customer => customer.Delete())
                .Then(customer => customer.Inactivate())
                .Then(customer => customer.IsValid.Should().BeTrue())
                .Then(customer => customer.Active.Should().BeFalse());

        [Fact]
        public void Update_Customer_ValidName()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .When(customer => (customer, request: new UpdateCustomerRequest()
                {
                    Name = new Bogus.DataSets.Name().FullName()
                }))
                .Then(tuple => tuple.customer.Update(tuple.request))
                .Then(tuple => tuple.customer.Name.Should().Be(tuple.request.Name))
                .Then(tuple => tuple.customer.BirthDate.Should().NotBe(DateOnly.FromDateTime(tuple.request.BirthDate ?? default)))
                .Then(tuple => tuple.customer.IsValid.Should().BeTrue());

        [Fact]
        public void Update_Customer_InvalidName()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .When(customer => (customer, request: new UpdateCustomerRequest()
                {
                    Name = new Bogus.DataSets.Lorem().Letter(2)
                }))
                .Then(tuple => tuple.customer.Update(tuple.request))
                .Then(tuple => tuple.customer.IsValid.Should().BeFalse());

        [Fact]
        public void Update_Customer_Phone()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .When(customer => (customer, request: new UpdateCustomerRequest()
                {
                    Contact = new Faker<UpdateCustomerRequest.UpdateContactRequest>("pt_BR")
                        .RuleFor(contact => contact.Phone, fake => fake.Phone.PhoneNumber())
                        .Generate()
                }))
                .Then(tuple => tuple.customer.Update(tuple.request))
                .Then(tuple => tuple.customer.Contact.Phone.Should().Be(tuple.request.Contact?.Phone))
                .Then(tuple => tuple.customer.IsValid.Should().BeTrue());

        [Fact]
        public void Update_Customer_Login()
            => BehaviorExtensions
                .Given(() => CreateDefaultFakeCustomer())
                .When(fakeRequest => (request: fakeRequest.Generate(), customer: new Customer()))
                .When(tuple => tuple.customer.Register(tuple.request))
                .When(tuple => tuple.customer)
                .When(customer => (customer, request: new UpdateCustomerRequest()
                {
                    Login = new Bogus.DataSets.Internet().UserName()
                }))
                .Then(tuple => tuple.customer.Update(tuple.request))
                .Then(tuple => tuple.customer.Credential.Login.Should().Be(tuple.request.Login))
                .Then(tuple => tuple.customer.IsValid.Should().BeTrue());

        private static Faker<CreateCustomerRequest> CreateDefaultFakeCustomer()
            => new Faker<CreateCustomerRequest>("pt_BR")
                .RuleFor(customer => customer.Name, fake => fake.Person.FullName)
                .RuleFor(customer => customer.BirthDate, fake => fake.Person.DateOfBirth)
                .RuleFor(customer => customer.Login, fake => fake.Person.UserName)
                .RuleFor(customer => customer.Password, fake => fake.Internet.Password())
                .RuleFor(customer => customer.Contact, fake 
                    => new Faker<CreateCustomerRequest.CreateContactRequest>("pt_BR")
                    .RuleFor(contact => contact.Phone, fake => fake.Person.Phone)
                    .RuleFor(contact => contact.Email, fake => fake.Person.Email)
                    .Generate());
    }
}
