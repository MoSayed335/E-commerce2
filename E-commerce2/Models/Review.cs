using ECommerce2.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce2.Models
{
    public class Review : BaseEntity
    {
        public string UserId { get; set; } = default!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        
        public bool HasPhoto { get; set; }
        public string? ImageUrl { get; set; }
        
        public bool IsPinned { get; set; }
        
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    }
}
