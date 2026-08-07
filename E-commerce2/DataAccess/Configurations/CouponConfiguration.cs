using ECommerce2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce2.DataAccess.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons");

            builder.Property(c => c.Code).IsRequired().HasMaxLength(30);
            builder.HasIndex(c => c.Code).IsUnique();

            builder.Property(c => c.Value).HasColumnType("decimal(18,2)");
            builder.Property(c => c.MinOrderAmount).HasColumnType("decimal(18,2)");
            builder.Property(c => c.MaxDiscountAmount).HasColumnType("decimal(18,2)");
        }
    }

    public class CouponProductConfiguration : IEntityTypeConfiguration<CouponProduct>
    {
        public void Configure(EntityTypeBuilder<CouponProduct> builder)
        {
            builder.ToTable("CouponProducts");
            builder.HasKey(cp => new { cp.CouponId, cp.ProductId });

            builder.HasOne(cp => cp.Coupon)
                   .WithMany(c => c.CouponProducts)
                   .HasForeignKey(cp => cp.CouponId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.Product)
                   .WithMany(p => p.CouponProducts)
                   .HasForeignKey(cp => cp.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class CouponCategoryConfiguration : IEntityTypeConfiguration<CouponCategory>
    {
        public void Configure(EntityTypeBuilder<CouponCategory> builder)
        {
            builder.ToTable("CouponCategories");
            builder.HasKey(cc => new { cc.CouponId, cc.CategoryId });

            builder.HasOne(cc => cc.Coupon)
                   .WithMany(c => c.CouponCategories)
                   .HasForeignKey(cc => cc.CouponId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Category)
                   .WithMany(cat => cat.CouponCategories)
                   .HasForeignKey(cc => cc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
