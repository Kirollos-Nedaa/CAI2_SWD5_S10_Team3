using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;


namespace TechXpress.Infrastructure.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Category_Id);
            
            builder.Property(c => c.Name)
                   .HasMaxLength(100);

            builder.Property(c => c.Img)
                     .IsRequired(false)
                     .HasMaxLength(255);
        }
    }
}
