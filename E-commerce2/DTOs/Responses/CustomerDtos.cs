namespace ECommerce2.DTOs.Responses
{
    public class AdminCustomerSummaryDto
    {
        public string Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
