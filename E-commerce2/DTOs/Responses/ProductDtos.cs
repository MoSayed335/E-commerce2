namespace ECommerce2.DTOs
{
    // بنستخدم DTOs بدل ما نرجّع الـ Entities مباشرة من الـ API، عشان:
    // 1. متسربش تفاصيل داخلية (زي IsDeleted) للعميل
    // 2. تقدر تغيّر شكل الـ Entity من غير ما تكسر الـ API Contract

    public record ProductListItemDto(
        int Id,
        string Name,
        decimal Price,
        string MainImageUrl,
        bool Status
    );

    public record ProductDetailsDto(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        decimal? DiscountPercentage,
        string CategoryName,
        List<string> ImageUrls,
        List<ProductVariantDto> Variants
    );

    public record ProductVariantDto(
        int Id,
        string? ColorName,
        string? ColorHex,
        string? SizeName,
        int Stock,
        decimal EffectivePrice
    );

    public record CreateProductDto(
        string Name,
        string? Description,
        decimal Price,
        int CategoryId,
        string MainImageUrl
    );
}
