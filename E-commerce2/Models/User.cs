using Microsoft.AspNetCore.Identity;

namespace ECommerce2.Models
{
    public class User : IdentityUser
    {
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }


        public DateTime? DateOfBirth { get; set; }

        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Cart? Cart { get; set; }
    }
    public class UserAddress : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public string FullAddress { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Landmark { get; set; }
        public bool IsDefault { get; set; }
    }
}

