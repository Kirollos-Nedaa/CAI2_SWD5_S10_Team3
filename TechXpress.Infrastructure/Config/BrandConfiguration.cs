using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(b => b.Brand_Id);
           
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(b => b.imgUrl)
                .IsRequired();

            builder.HasMany(b => b.Product)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.Brand_Id);
        }
    }
}
