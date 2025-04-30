using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount_Sales>
    {
        public void Configure(EntityTypeBuilder<Discount_Sales> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Start_Date)
                .IsRequired()
                .HasColumnType("date");
            builder.Property(d => d.End_Date)
                .IsRequired()
                .HasColumnType("date");
            builder.Property(d => d.Sale_amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
