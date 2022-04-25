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

## Let's Code!
To my domain I represent it like this:
![domain_layer](/imgs/domain_layer.png)

And I have my Aggregates and Value Objects that contain my business logics, properties, and behaviors:
![aggregate](/imgs/aggregate.png)

So, this is my application's core! So I need to guarantee this is works very fine and for it, I need to cover a hundred percent with tests! And to do this, I can use of Unit Test scope:
![unit_test_project](/imgs//unit_test_project.png)

And I can use a behavior strategy to test all my scenario, like this:
```
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
}
```
