using ECommerce2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce2.Models
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]

        public User User { get; set; } = default!;

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }

    public class CartItem : BaseEntity
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; } = default!;

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = default!;

        public int Quantity { get; set; }
    }
}
