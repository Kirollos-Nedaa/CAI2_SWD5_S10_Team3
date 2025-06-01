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
    public class Order_ItemConfiguration : IEntityTypeConfiguration<Order_Item>
    {
        public void Configure(EntityTypeBuilder<Order_Item> builder)
        {
            builder.HasKey(oi => oi.Order_Item_Id);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Order_Items)
                .HasForeignKey(oi => oi.Order_Id);
            
            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.Product_Id);
            
            builder.Property(oi => oi.Quantity)
                .IsRequired();
            
            builder.Property(oi => oi.Total)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
