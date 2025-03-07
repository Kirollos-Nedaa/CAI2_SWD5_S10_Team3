using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.ProductId);


            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Quantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.ImageUrl)
                .IsRequired();
        }
    }
}
