﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class InventoryConfigurations : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.InventoryId);

            builder.HasOne(i => i.Product)
                   .WithMany(p => p.Inventories) 
                   .HasForeignKey(i => i.ProductId);

            builder.Property(i => i.Amount_on_hand)
                   .IsRequired();
        }
    }
}
