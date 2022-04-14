using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using Test.Scopes.Domain.Aggregates.Customers;
using Test.Scopes.Domain.ValueObjects.Contacts;
using Test.Scopes.Domain.ValueObjects.Credentials;

namespace Test.Scopes.Infra.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .HasKey(customer => customer.Id);

            builder
                .Property(customer => customer.Id)
                .UseIdentityColumn();

            builder
                .Property(customer => customer.Active)
                .IsRequired()
                .HasDefaultValue(true);

            builder
                .Property(customer => customer.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder
                .Property(customer => customer.CreateAt)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);


            builder
                .Property(customer => customer.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(customer => customer.BirthDate)
                .IsRequired()
                .HasConversion(
                    dateOnly => dateOnly.ToDateTime(default), 
                    dateTime => DateOnly.FromDateTime(dateTime));

            builder
                .OwnsOne(customer => customer.Credential);

            builder
                .OwnsOne(customer => customer.Contact);
        }
    }
}
