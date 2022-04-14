using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Test.Scopes.Infra.Contexts;

namespace Test.Scopes.IntegrationTest.Fixture
{
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
}
