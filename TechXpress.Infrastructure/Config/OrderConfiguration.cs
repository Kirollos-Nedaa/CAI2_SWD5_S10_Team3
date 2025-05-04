using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Order_Id);

            builder.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.Customer_Id);

            builder.Property(o => o.Order_Date)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(o => o.Total_Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.Shipping_Address_Id);

            builder.Property(o => o.Order_Status)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
