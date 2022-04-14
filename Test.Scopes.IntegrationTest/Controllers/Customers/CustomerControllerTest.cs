using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Models.Customers.CreateCustomer;
using Test.Scopes.Domain.Models.Customers.RecoverCustomer;
using Test.Scopes.Domain.Models.Customers.UpdateCustomer;
using Test.Scopes.Infra.Contexts;
using Test.Scopes.IntegrationTest.Fixture;
using Xunit;

namespace Test.Scopes.IntegrationTest.Controllers.Customers;

public class CustomerControllerTest
{
    [Fact]
    public async Task RegisterCustomer()
    {
        await using var api = new TestScopesApiFixture();
        var client = api.CreateClient();
        var request = new Faker<CreateCustomerRequest>("pt_BR")
            .RuleFor(customer => customer.Name, fake => fake.Person.FullName)
            .RuleFor(customer => customer.BirthDate, fake => fake.Person.DateOfBirth)
            .RuleFor(customer => customer.Login, fake => fake.Person.UserName)
            .RuleFor(customer => customer.Password, fake => fake.Internet.Password())
            .RuleFor(customer => customer.Contact, fake
                => new Faker<CreateCustomerRequest.CreateContactRequest>("pt_BR")
                .RuleFor(contact => contact.Phone, fake => fake.Person.Phone)
                .RuleFor(contact => contact.Email, fake => fake.Person.Email)
                .Generate())
            .Generate();

        var response = await client.PostAsJsonAsync("/api/Customer", request);
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task RecoverCustomer()
    {
        await using var api = new TestScopesApiFixture();
        var client = api.CreateClient();

        var createCustomerRequest = new Faker<CreateCustomerRequest>("pt_BR")
            .RuleFor(customer => customer.Name, fake => fake.Person.FullName)
            .RuleFor(customer => customer.BirthDate, fake => fake.Person.DateOfBirth)
            .RuleFor(customer => customer.Login, fake => fake.Person.UserName)
            .RuleFor(customer => customer.Password, fake => fake.Internet.Password())
            .RuleFor(customer => customer.Contact, fake
                => new Faker<CreateCustomerRequest.CreateContactRequest>("pt_BR")
                .RuleFor(contact => contact.Phone, fake => fake.Person.Phone)
                .RuleFor(contact => contact.Email, fake => fake.Person.Email)
                .Generate())
            .Generate();

        using var scope = api.Services.CreateScope();
        var provider = scope.ServiceProvider;
        var handler = provider.GetRequiredService<ICreateCustomerHandler>();
        await handler.Handle(createCustomerRequest, default);

        var response = await client.GetAsync("/api/Customer");
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}