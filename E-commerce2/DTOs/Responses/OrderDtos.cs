namespace ECommerce2.DTOs
{
    public record CreateOrderItemDto(int ProductVariantId, int Quantity);

    public record CreateOrderDto(

        int UserAddressId,
        string? CouponCode,
        string? Notes,
        List<CreateOrderItemDto> Items
    );

    public record CheckoutDto(
        int UserAddressId,
        string? CouponCode,
        string? Notes
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
        string? AdminNotes,
        List<OrderItemDto> Items
    );
    public record AdminOrderSummaryDto(
        int Id,
        string OrderNumber,
        string Status,
        string CustomerName,
        string CustomerPhone,
        string ShippingCity,
        string ShippingAddress,
        decimal TotalPrice,
        DateTime CreatedAt,
        string? AdminNotes,
        List<OrderItemDto> Items
    );

    public record OrderQueryParameters(
        ECommerce2.Models.Enums.OrderStatus? Status = null,
        string? SearchQuery = null,
        int PageIndex = 1,
        int PageSize = 10
    );

    public record OrderTrackingStage(
        string Name,
        string Description,
        bool IsCompleted,
        DateTime? CompletedAt
    );

    public record OrderTrackerDto(
        string OrderNumber,
        string CurrentStatus,
        decimal TotalAmount,
        string ShippingAddress,
        List<OrderTrackingStage> Stages
    );
}
