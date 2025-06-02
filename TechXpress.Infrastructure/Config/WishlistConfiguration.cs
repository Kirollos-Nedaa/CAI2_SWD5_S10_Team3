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
            builder.HasKey(w => w.Wishlist_Id);

            builder.HasOne(w => w.User)
                   .WithMany()
                   .HasForeignKey(w => w.Customer_Id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Product)
                   .WithMany(p => p.Wishlists)
                   .HasForeignKey(w => w.Product_Id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Wishlist");
        }
    }
}