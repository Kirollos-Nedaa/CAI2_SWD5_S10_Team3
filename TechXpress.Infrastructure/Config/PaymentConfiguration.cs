using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechXpress.Domain.Models;

namespace TechXpress.Infrastructure.Config
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Payment_Id);

            builder.HasOne(p => p.Orders)
                .WithOne()
                .HasForeignKey<Payment>(p => p.Oreder_Id);

            builder.Property(p => p.Stripe_payment_intent_id)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Payment_type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Payment_date)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
