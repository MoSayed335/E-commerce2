using ECommerce2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce2.DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            builder.HasIndex(p => p.Sku).IsUnique();

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.DiscountPercentage).HasColumnType("decimal(5,2)");

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }

    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.Property(v => v.Sku).IsRequired().HasMaxLength(50);
            builder.HasIndex(v => v.Sku).IsUnique();
            builder.Property(v => v.PriceOverride).HasColumnType("decimal(18,2)");

            builder.HasOne(v => v.Product)
                   .WithMany(p => p.Variants)
                   .HasForeignKey(v => v.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Color)
                   .WithMany(c => c.ProductVariants)
                   .HasForeignKey(v => v.ColorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Size)
                   .WithMany(s => s.ProductVariants)
                   .HasForeignKey(v => v.SizeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(v => new { v.ProductId, v.ColorId, v.SizeId }).IsUnique();
        }
    }

    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasOne(i => i.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
