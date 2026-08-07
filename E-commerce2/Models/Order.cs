using ECommerce2.Models;
using ECommerce2.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce2.Models
{

    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = default!;

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;

        public string ShippingAddress { get; set; } = default!;
        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; } = default!;
        public string? Notes { get; set; }
        public string? AdminNotes { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal SubTotal { get; set; }    
        public decimal DeliveryFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }   

        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal UnitPriceAtPurchase { get; set; }

        public decimal Subtotal => UnitPriceAtPurchase * Quantity;
    }
}
