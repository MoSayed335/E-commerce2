namespace ECommerce2.DTOs
{
    public record CreateOrderItemDto(int ProductVariantId, int Quantity);

    public record CreateOrderDto(
        string UserId,
        int UserAddressId,
        string? CouponCode,
        string? Notes,
        List<CreateOrderItemDto> Items
    );

    public record OrderItemDto(
        string ProductName,
        string? ColorName,
        string? SizeName,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );

    public record OrderDetailsDto(
        int Id,
        string OrderNumber,
        string Status,
        decimal SubTotal,
        decimal DiscountAmount,
        decimal DeliveryFee,
        decimal TotalPrice,
        string ShippingAddress,
        DateTime CreatedAt,
        List<OrderItemDto> Items
    );
}
