namespace ECommerce2.Models
{
    public class Governorate : BaseEntity
    {
        public string NameEn { get; set; } = default!;
        public string NameAr { get; set; } = default!;
        public decimal Fee { get; set; }
        public string EstimatedDelivery { get; set; } = default!;

        public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
