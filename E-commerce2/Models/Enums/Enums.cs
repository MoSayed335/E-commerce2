namespace ECommerce2.Models.Enums
{
    public enum OrderStatus
    {
        Pending = 0,          // لسه متأكدش
        Confirmed = 1,        // اتأكد من العميل (تليفونيًا مثلاً)
        Processing = 2,       // بيتجهز في المخزن
        Shipped = 3,          // خرج مع شركة الشحن
        OutForDelivery = 4,   // مع المندوب
        Delivered = 5,        // اتسلم واتحصل تحصيله (COD)
        Cancelled = 6,        // اتلغى قبل الشحن
        Returned = 7,         // رجع بعد التسليم
        DeliveryFailed = 8    // فشل التحصيل / رفض الاستلام
    }

    public enum DiscountType
    {
        Percentage = 0,
        FixedAmount = 1
    }

    public enum BannerType
    {
        General = 0,
        Offer = 1,
        Sale = 2
    }

    public enum CouponScope
    {
        Global = 0,
        SpecificProducts = 1,
        SpecificCategories = 2
    }
}
