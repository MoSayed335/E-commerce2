namespace ECommerce2.DTOs
{
    public record AddToCartDto(
        int ProductVariantId,
        int Quantity
    );

    public record UpdateCartItemDto(
        int Quantity
    );

    public record CartItemDto(
        int Id,
        int ProductVariantId,
        string ProductName,
        string? ColorName,
        string? SizeName,
        string? ImageUrl,
        decimal UnitPrice,
        int Quantity,
        decimal Subtotal
    );

    public record CartDto(
        int Id,
        decimal TotalPrice,
        List<CartItemDto> Items
    );
}
