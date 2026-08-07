using ECommerce2.Models.Enums;

namespace ECommerce2.DTOs
{
    public record CreateCouponDto(
        string Code,
        string? Name,
        DiscountType DiscountType,
        decimal Value,
        decimal? MinOrderAmount,
        decimal? MaxDiscountAmount,
        int? UsageLimit,
        DateTime StartDate,
        DateTime? ExpiryDate
    );

    public record AdminCouponSummaryDto(
        int Id,
        string Code,
        string? Name,
        DiscountType DiscountType,
        decimal Value,
        decimal? MinOrderAmount,
        decimal? MaxDiscountAmount,
        int? UsageLimit,
        int TimesUsed,
        DateTime StartDate,
        DateTime? ExpiryDate,
        bool Status
    );

    public record CampaignTrackingDto(
        string SourceName,
        int Redemptions
    );

    public record CouponQueryParameters(
        string? SearchQuery = null,
        bool? Status = null,
        int PageIndex = 1,
        int PageSize = 10
    );
}
