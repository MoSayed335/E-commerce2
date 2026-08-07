using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce2.Models
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; } = default!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
