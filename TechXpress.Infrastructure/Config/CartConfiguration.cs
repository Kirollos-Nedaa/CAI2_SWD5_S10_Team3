using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Cart_ID);

            builder.Property(c => c.Quantity)
                   .IsRequired();

            builder.Property(c => c.Total)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne<Customer>()  
                   .WithMany()
                   .HasForeignKey(c => c.Customer_ID) // check if one to one or one to many
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
