using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.Product_Id);

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
                .IsRequired();

            builder.Property(p => p.ImageUrl);

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Product)
                .HasForeignKey(p => p.Category_Id);

            builder.HasOne(p => p.Brand)
                .WithMany(p => p.Product)
                .HasForeignKey(p => p.Brand_Id);
        }
    }
}
