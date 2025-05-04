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
    public class Discount_ItemConfiguration : IEntityTypeConfiguration<Discount_Item>
    {
        public void Configure(EntityTypeBuilder<Discount_Item> builder)
        {
            builder.HasKey(di => di.Discount_Item_Id);

            builder.HasOne(di => di.Discount)
                .WithMany(d => d.Discount_Items)
                .HasForeignKey(di => di.Discount_Id);

            builder.HasOne(di => di.Product)
                .WithMany(p => p.Discount_Items)
                .HasForeignKey(di => di.Product_Id);
        }
    }
}
