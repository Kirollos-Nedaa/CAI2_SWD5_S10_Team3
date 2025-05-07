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
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => new { w.Customer_Id, w.Product_Id });

            builder.HasOne(w => w.Customer)
                   .WithMany(c => c.Wishlists)
                   .HasForeignKey(w => w.Customer_Id);

            builder.HasOne(w => w.Product)
                   .WithMany(p => p.Wishlists)
                   .HasForeignKey(w => w.Product_Id);

            builder.ToTable("Wishlist");
        }
    }
}