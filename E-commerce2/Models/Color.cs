using ECommerce2.Models;

namespace ECommerce2.Models
{

    public class Color : BaseEntity
    {
        public string Name { get; set; } = default!;   // أحمر
        public string HexCode { get; set; } = default!; // "#FF0000"

        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }

   
    public class Size : BaseEntity
    {
        public string Name { get; set; } = default!; //"XL", "M", "42"
        public int SortOrder { get; set; }            // S, M, L, XL

        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
