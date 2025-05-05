using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class Cart_ItemsConfiguration : IEntityTypeConfiguration<Cart_Items>
    {
        public void Configure(EntityTypeBuilder<Cart_Items> builder)
        {
            builder.HasKey(ci => ci.Cart_Item_Id);

            builder.HasOne(ci => ci.Cart)
                .WithMany(ci => ci.Cart_Items)
                .HasForeignKey(ci => ci.Cart_Id);

            builder.HasOne(ci => ci.Product)
                .WithMany(ci => ci.Cart_Items)
                .HasForeignKey(ci => ci.Product_Id);

            builder.Property(ci => ci.Quantity)
                .IsRequired();
        }
    }
}
