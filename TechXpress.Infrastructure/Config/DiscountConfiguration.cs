using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.Discount_Id);
            
            builder.Property(d => d.Sale_amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            builder.Property(d => d.Start_Date)
                .IsRequired()
                .HasColumnType("date");
            
            builder.Property(d => d.End_Date)
                .IsRequired()
                .HasColumnType("date");
        }
    }
}
