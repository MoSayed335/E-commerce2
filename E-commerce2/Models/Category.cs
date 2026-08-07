using ECommerce2.Models;

namespace ECommerce2.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool Status { get; set; } = true;

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<CouponCategory> CouponCategories { get; set; } = new List<CouponCategory>();
        public ICollection<BannerCategory> BannerCategories { get; set; } = new List<BannerCategory>();
    }
}
