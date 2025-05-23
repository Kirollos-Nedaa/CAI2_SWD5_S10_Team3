﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.HasKey(o => o.Order_Id);

            builder.HasOne(o => o.ApplicationUser)
                .WithMany()
                .HasForeignKey(o => o.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Order_Date)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(o => o.Total_Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.Shipping_Address_Id)
                .HasPrincipalKey(a => a.Address_Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Property(o => o.Order_Status)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
