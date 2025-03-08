using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class AddressesConfiurations : IEntityTypeConfiguration<Addresses>
    {
        public void Configure(EntityTypeBuilder<Addresses> builder)
        {
            builder.HasKey(a => a.AddressId);

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Apartment)
                   .HasMaxLength(50);

            builder.Property(a => a.PostCode)
                   .HasMaxLength(20);

            builder.Property(a => a.IsDefault)
                   .IsRequired();

            builder.HasOne(a => a.Customer)
                   .WithMany()
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}