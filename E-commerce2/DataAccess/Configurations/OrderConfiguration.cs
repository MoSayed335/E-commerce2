using ECommerce2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce2.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(30);
            builder.HasIndex(o => o.OrderNumber).IsUnique();

            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.DeliveryFee).HasColumnType("decimal(18,2)");
            builder.Property(o => o.DiscountAmount).HasColumnType("decimal(18,2)");
            builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Coupon)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CouponId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .IsRequired(false);
        }
    }

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.Property(oi => oi.UnitPriceAtPurchase).HasColumnType("decimal(18,2)");
            builder.Ignore(oi => oi.Subtotal); // خاصية محسوبة، مش عمود فعلي

            // Order -> OrderItems: لو الأوردر اتمسح، الأصناف بتاعته تتمسح معاه
            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.Items)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ProductVariant -> OrderItems: مينفعش تمسح Variant اتباع فعلاً في أوردر قديم
            builder.HasOne(oi => oi.ProductVariant)
                   .WithMany(v => v.OrderItems)
                   .HasForeignKey(oi => oi.ProductVariantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
