using ECommerce2.Models;
using ECommerce2.Models.Enums;

namespace ECommerce2.Models
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = default!;
        public string? Name { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal Value { get; set; }
        public CouponScope Scope { get; set; } = CouponScope.Global;

        public decimal? MinOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }

        public int? UsageLimit { get; set; }
        public int TimesUsed { get; set; } = 0;

        public DateTime StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Status { get; set; } = true;

        public ICollection<CouponProduct> CouponProducts { get; set; } = new List<CouponProduct>();
        public ICollection<CouponCategory> CouponCategories { get; set; } = new List<CouponCategory>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
    public class CouponProduct
    {
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }

    public class CouponCategory
    {
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; } = default!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}
