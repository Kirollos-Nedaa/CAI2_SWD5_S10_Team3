using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Cart_Id);

            builder.HasOne(c => c.Customer)
                .WithOne()
                .HasForeignKey<Cart>(c => c.Customer_Id);

            builder.Property(c => c.Created_At)
                .IsRequired()
                .HasColumnType("datetime");
        }
    }
}
