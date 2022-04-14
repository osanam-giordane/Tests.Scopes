using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;
using Test.Scopes.Domain.Aggregates.Customers;
using Test.Scopes.Domain.ValueObjects.Contacts;
using Test.Scopes.Domain.ValueObjects.Credentials;

namespace Test.Scopes.Infra.Contexts;

public class PersistenceDbContext : DbContext
{
    private const string SqlLatin1GeneralCp1CsAs = "SQL_Latin1_General_CP1_CS_AS";
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public PersistenceDbContext(DbContextOptions options, IConfiguration configuration, ILoggerFactory loggerFactory)
        : base(options)
        => (_configuration, _loggerFactory) = (configuration, loggerFactory);

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Credential> Credentials => Set<Credential>();
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation(SqlLatin1GeneralCp1CsAs);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .UseSqlServer(
                connectionString: _configuration.GetConnectionString("Persistence"))
            .UseLoggerFactory(_loggerFactory);
    }
}