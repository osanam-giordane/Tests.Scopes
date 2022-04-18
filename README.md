# Test Scopes

To cover our application, we have many types of tests for it. So, before we start to talk about this application, let's analyze the test pyramid:

![test pyramid](https://res.cloudinary.com/practicaldev/image/fetch/s--dcM0135C--/c_limit%2Cf_auto%2Cfl_progressive%2Cq_auto%2Cw_880/https://dev-to-uploads.s3.amazonaws.com/i/ealtg2m79jiaur0ryb3v.png)

This simple pyramid we have three kind of tests:

* End-to-End: this scope allow we to test our UI
* Integration: this scope allow we to test the providers integrations, like APIs, Databases and Queues.
* Unit: this scope allow we to test our business logic, the application's heart.

But, you can ask me "Why can we not use just Unit Test? It's simple, we need just mock somethings, and go on!"... 🤔

So, the answer it's simple too 😁 But I don't tell you a simple answer, let's think the below scenario:

Imagine that your application has a hundred percent of coverage, and all the tests was built on Unit Scopes, You test the providers methods, entry points and your business logical. It's simple, easy and "fast" to do it. But, in a moment, you need to change a provider. A big part of your test, reference the provider changed, will fail 😕, because you don't use it in the best way...

To explain this, in more details, let's see this project!

---
### About the project
I built this project based on Domain-Driven Design. So, in my API, I have a layer that represents my business logic, and there, I've Aggregates, Entities and Value Objects (in this case, I just have an Aggregate and Value Objects): 

![domain layer](/imgs/domain_layer.png)

Besides that, I've other layers that represent a connection to provider and a layer to represent my application service, that use Domain and Provider to do something:

![other layers](/imgs/other_layers.png)