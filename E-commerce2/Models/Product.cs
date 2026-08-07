using ECommerce2.Models;

namespace ECommerce2.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool Status { get; set; } = true; // متاح / غير متاح

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public string MainImageUrl { get; set; } = default!;

        // Navigation Properties
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<CouponProduct> CouponProducts { get; set; } = new List<CouponProduct>();
        public ICollection<BannerProduct> BannerProducts { get; set; } = new List<BannerProduct>();
    }

    /// <summary>
    /// SubImgs في الـ ERD الأصلي كانت attribute واحدة، وده يمنع تخزين أكتر من صورة.
    /// جدول منفصل يسمح بعدد غير محدود من الصور لكل منتج.
    /// </summary>
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// دي الحل الأساسي لمشكلة Color/Size كـ Multivalued Attributes.
    /// كل توليفة (منتج + لون + مقاس) بقت لها صف مستقل بمخزون وSKU خاص بيها.
    /// لو منتج مالوش ألوان/مقاسات، ممكن يتسجّل Variant واحد بس بـ ColorId/SizeId = null.
    /// </summary>
    public class ProductVariant : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int? ColorId { get; set; }
        public Color? Color { get; set; }

        public int? SizeId { get; set; }
        public Size? Size { get; set; }

        public string Sku { get; set; } = default!;
        public int Stock { get; set; }

        // اختياري: لو الـ Variant ده سعره مختلف عن سعر المنتج الأساسي
        public decimal? PriceOverride { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
