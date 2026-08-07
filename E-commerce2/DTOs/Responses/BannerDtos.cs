using ECommerce2.Models.Enums;

namespace ECommerce2.DTOs
{
    public record CreateBannerDto(
        string Name,
        string? Description,
        string ImageUrl,
        BannerType Type,
        bool Status,
        DateTime StartAt,
        DateTime? EndAt,
        List<int> ProductIds,
        List<int> CategoryIds
    );

    public record UpdateBannerDto(
        string Name,
        string? Description,
        string ImageUrl,
        BannerType Type,
        bool Status,
        DateTime StartAt,
        DateTime? EndAt,
        List<int> ProductIds,
        List<int> CategoryIds
    );

    public record BannerDto(
        int Id,
        string Name,
        string? Description,
        string ImageUrl,
        BannerType Type,
        bool Status,
        DateTime StartAt,
        DateTime? EndAt,
        List<int> ProductIds,
        List<int> CategoryIds
    );

    public record BannerQueryParameters(
        bool? Status = null,
        BannerType? Type = null,
        string? SearchQuery = null,
        int PageIndex = 1,
        int PageSize = 10
    );
}
