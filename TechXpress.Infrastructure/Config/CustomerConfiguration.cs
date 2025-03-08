using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.PhNo)
                .HasMaxLength(15);

            builder.Property(c => c.Password)
                .IsRequired();

            builder.Property(c => c.DateOfBirth)
               .IsRequired()
               .HasConversion(
                   v => v.ToDateTime(TimeOnly.MinValue),
                   v => DateOnly.FromDateTime(v)
               )
               .HasColumnType("timestamp");
        }
    }
}
