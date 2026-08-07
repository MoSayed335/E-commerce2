using ECommerce2.Models.Enums;

namespace ECommerce2.DTOs
{
    public record AdminReviewSummaryDto(
        int Id,
        string ReviewerName,
        string ProductName,
        int Rating,
        string Comment,
        bool HasPhoto,
        string? ImageUrl,
        bool IsPinned,
        string Status,
        DateTime CreatedAt
    );

    public record ReviewQueryParameters(
        string? SearchQuery = null,
        ReviewStatus? Status = null,
        bool? IsPinned = null,
        int PageIndex = 1,
        int PageSize = 10
    );

    public record UpdateLiveUrgencyDto(
        int BaseCounter
    );

    public record UpdateReviewStatusRequest(
        ReviewStatus Status
    );

    public record StorefrontReviewDto(
        int Id,
        string ReviewerName,
        int Rating,
        string Comment,
        string? ImageUrl,
        bool IsPinned,
        DateTime CreatedAt
    );

    public record CreateReviewDto(
        int ProductId,
        int Rating,
        string Comment,
        string? ImageUrl
    );
}
