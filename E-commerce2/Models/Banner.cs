using ECommerce2.Models;
using ECommerce2.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce2.Models
{
    public class Banner : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = default!;
        public BannerType Type { get; set; } = BannerType.General;
        public bool Status { get; set; } = true;

        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public ICollection<BannerProduct> BannerProducts { get; set; } = new List<BannerProduct>();
        public ICollection<BannerCategory> BannerCategories { get; set; } = new List<BannerCategory>();
    }

    public class BannerProduct
    {
        [Key]
        public int BannerId { get; set; }
        public Banner Banner { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }

    public class BannerCategory
    {
        [Key]
        public int BannerId { get; set; }
        public Banner Banner { get; set; } = default!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}
