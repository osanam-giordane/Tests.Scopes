using Bogus;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Test.Scopes.Domain.Interfaces.Handlers;
using Test.Scopes.Domain.Interfaces.Persistence;
using Test.Scopes.Domain.Models.Customers.CreateCustomer;
using Test.Scopes.Infra.Contexts;
using Test.Scopes.Infra.Persistence;
using Test.Scopes.Services.Customers;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    options.ValidateOnBuild = true;
});

builder.WebHost.ConfigureAppConfiguration(configurationBuilder =>
{
    configurationBuilder
        .AddEnvironmentVariables();
});

builder.Services
    .AddControllers()
    .AddFluentValidation(cfg =>
    {
        cfg.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        cfg.ImplicitlyValidateChildProperties = false;
    });

builder.Services
    .AddScoped<DbContext, PersistenceDbContext>()
    .AddDbContext<PersistenceDbContext>();

builder.Services
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<ICustomerService, CustomerService>();

builder.Services
    .AddScoped<ICreateCustomerHandler, CreateCustomerHandler>()
    .AddScoped<IActivateCustomerHandler, ActivateCustomerHandler>()
    .AddScoped<IInactivateCustomerHandler, InactivateCustomerHandler>()
    .AddScoped<IDeleteCustomerHandler, DeleteCustomerHandler>()
    .AddScoped<IUpdateCustomerHandler, UpdateCustomerHandler>()
    .AddScoped<IRecoverCustomersHandler, RecoverCustomersHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() ||
    app.Environment.IsStaging())
{
    using var scope = app.Services.CreateScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

    if (dbContext.Database.IsRelational())
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        await dbContext.Database.EnsureCreatedAsync();

        await DataSeed(scope.ServiceProvider.GetRequiredService<ICreateCustomerHandler>());
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting().UseCors(e =>
{
    e.AllowAnyOrigin();
    e.AllowAnyMethod();
    e.AllowAnyHeader();
});

app.Run();

static async Task DataSeed(ICreateCustomerHandler handler)
{
    var requests = new Faker<CreateCustomerRequest>("pt_BR")
        .RuleFor(customer => customer.Name, fake => fake.Person.FullName)
        .RuleFor(customer => customer.BirthDate, fake => fake.Person.DateOfBirth)
        .RuleFor(customer => customer.Login, fake => fake.Person.UserName)
        .RuleFor(customer => customer.Password, fake => fake.Internet.Password())
        .RuleFor(customer => customer.Contact, fake
            => new Faker<CreateCustomerRequest.CreateContactRequest>("pt_BR")
            .RuleFor(contact => contact.Phone, fake => fake.Person.Phone)
            .RuleFor(contact => contact.Email, fake => fake.Person.Email)
            .Generate())
        .Generate(5);

    foreach (var request in requests)
        await handler.Handle(request, default);
}