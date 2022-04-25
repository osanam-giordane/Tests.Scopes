# Test Scopes

To cover our application, we have many types of tests for it. So, let's analyze the test pyramid:

![test pyramid](https://res.cloudinary.com/practicaldev/image/fetch/s--dcM0135C--/c_limit%2Cf_auto%2Cfl_progressive%2Cq_auto%2Cw_880/https://dev-to-uploads.s3.amazonaws.com/i/ealtg2m79jiaur0ryb3v.png)

This simple pyramid we have three kind of tests:

* End-to-End: this scope allow we to test our UI
* Integration: this scope allow we to test the providers integrations, like APIs, Databases and Queues.
* Unit: this scope allow we to test our business logic, the application's heart.

But, you can ask me "Why can we not use just Unit Test? It's simple, we need just mock somethings, and go on!"... 🤔

So, the answer it's simple too 😁 But I don't tell you a simple answer, let's think the below scenario:

Imagine that your application has a hundred percent of coverage, and all the tests was built on Unit Scopes: You test the providers methods, entry points and your business logical. It's simple, easy and "fast" to do it. But, if in a moment, you need to change a provider. A big part of your test, reference the provider changed, will fail 😕, because you don't use it in the best way...

Looking for our project, how can we divide the tests to cover all application?

1. For the core/domain: 
	If we were guided for use Domain-Driven Design, we'll have Aggregates, Entities, and Value Objects that contain our representative world: business rules, properties, and behaviors. So it's interesting we cover a hundred percent and for it we can use Unit Tests, that is low cost and fast to build.
2. For the providers:
	The service layer uses the domain and providers to do something in our application. So, to guarantee the connection for these providers, we can get some main use cases to test the connections, like message brokers, APIs, and databases. We can use some In Memory strategies or mock the connection in our dependency injection to validate our connection configuration, and for it, we can use Fixture.
3. For complete flow:
	Our application has a front-end client. For it, we can use the e2e test! And to build this, we can containerize the providers, configure all our environment in a docker-compose, and run it in our pipeline.

See the next chapter for a look at how we can made all this tests in practice! 👀

---

## [Let's Code!] Unit Test Scope 🔥
To my domain I represent it like this:

![domain_layer](/imgs/domain_layer.png)

And I have my Aggregates and Value Objects that contain my business logics, properties, and behaviors:

```csharp
public class Customer : AggregateRoot<long>
{
    public string Name { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public Credential Credential { get; private set; }

    public Contact Contact { get; private set; }

    public void Register(CreateCustomerRequest request)
    {
        Name = request.Name;
        BirthDate = DateOnly.FromDateTime(request.BirthDate);
        Active = request.Active;

        Credential = new Credential(request.Login, request.Password);
        Contact = new Contact(request.Contact.Email, request.Contact.Phone);
        
        Validate();
    }

    public void Inactivate()
        => Active = false;

    public void Activate()
    {
        if (IsDeleted is true)
            return;

        Active = true;
    }

    protected override bool Validate()
        => OnValidate<CustomerValidator, Customer>();

	// ...
}
```

So, this is my application's core! So I need to guarantee this is works very fine and for it, I need to cover a hundred percent with tests! And to do this, I can use of Unit Test scope:

![unit_test_project](/imgs//unit_test_project.png)

And I can use a behavior strategy to test all my scenario, like this:

```csharp
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

	// ...
}
```

And with a few lines, I can cover all my aggregate (and my value objects, consequently)!

---

## [Let's Code!] Integration Test Scope 🔥

I need to certify that all my providers are configured correctly either. 

So, I have a specific project for it:

![integration_test_project](/imgs//integration_test_project.png)

And for it, I can use the Providers In Memory strategy to verify that my configurations are correct, using Fixtures:

```csharp
class TestScopesApiFixture : WebApplicationFactory<Program>
{
	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			services.RemoveAll(typeof(DbContextOptions<PersistenceDbContext>));
			services.AddScoped<DbContext, PersistenceDbContext>();
			services.AddDbContext<PersistenceDbContext>(options
				=> options.UseInMemoryDatabase("integration_test", new InMemoryDatabaseRoot()));
		});

		return base.CreateHost(builder);
	}
}
```

I remake my dependency injections and inject mine in-memory providers.

And use like below:

```csharp
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
}
```

For this kind of test, I just do it for main use cases like registering and updating customer. Made like it, I guarantee that providers work fine and my configurations too (the configuration are the same, for real or in-memory providers, for the same type).

---

## [Let's Code!] E2E Test Scope 🔥

Destinate to verify the complex flows and analyze the application behaviors!

![e2e_test_project](/imgs/e2e_test_project.png)

For this, I choose the main use cases and try to cover it, simulate a custmer behavior:

```js
context('Customers', () => {
    beforeEach(() => {
        cy.visit('https://localhost:7184/customer')
    })

    describe('Verify customers registered', () => {
        it('Contains 5 customers in list', () => verifyList(5))
    })

    describe('Register customers', () => {
        beforeEach(() => cy.get('#create_customer').click())

        it('With a valid fields', () => {
            cy.get('#login').type('joaodasilva')
            cy.get('#password').type('12345678')
            cy.get('#name').type('João da Silva')
            cy.get('#birth_date').type('1990-02-01')
            cy.get('#email').type('joao@gmail.com')
            cy.get('#phone').type('(19) 9880-0102')

            cy.get('#create_customer_modal').click()

            verifyList(6)
        })

        it('With invalid name', () => {
            cy.get('#login').type('joaodasilva')
            cy.get('#password').type('12345678')
            cy.get('#name').type('JP')
            cy.get('#birth_date').type('1990-02-01')
            cy.get('#email').type('joao@gmail.com')
            cy.get('#phone').type('(19) 9880-0102')

            cy.get('#create_customer_modal').click()

            verifyList(6)
        })
    })
})
```

---

## On Pipeline! 🧿

---

## Summing up
* Unit Tests
	* test just my domain
* Integration Tests
	* verify the providers configuration
	* isolated use cases
	* application servicer behaviors
	* ex.: how many times the provider was called?
* E2E tests:
	* most critical use cases
	* complete flow
		* ex.: since interface until storage data

---

# References:
- [The Practical Test Pyramid - Martin Fowler](https://martinfowler.com/articles/practical-test-pyramid.html)
- [E2E Testing - Code With Engineering Playbook](https://microsoft.github.io/code-with-engineering-playbook/automated-testing/e2e-testing/)
- [Unit vs Integration vs System vs E2E Testing - Code With Engineering Playbook](https://microsoft.github.io/code-with-engineering-playbook/automated-testing/e2e-testing/testing-comparison/)
- [Domain-Driven Design & Unit Tests](https://www.jamesmichaelhickey.com/ddd-unit-tests/)