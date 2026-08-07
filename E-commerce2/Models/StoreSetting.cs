namespace ECommerce2.Models
{
    public class StoreSetting : BaseEntity
    {
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
