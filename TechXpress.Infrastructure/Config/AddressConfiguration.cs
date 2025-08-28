using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasIndex(a => new { a.ApplicationUserId, a.IsDefault })
                  .IsUnique()
                  .HasFilter("[IsDefault] = 1");

            builder.HasKey(a => a.Address_Id);

            builder.HasOne(a => a.ApplicationUser)
                   .WithMany()
                   .HasForeignKey(a => a.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(a => a.AddressType)
                   .HasMaxLength(50);

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.StreetAddress)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(a => a.Buillding)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Floor)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.Flat)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.PostCode)
                   .HasMaxLength(20);

            builder.Property(a => a.IsDefault)
                   .IsRequired();
        }
    }
}