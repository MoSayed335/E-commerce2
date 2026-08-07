namespace ECommerce2.DTOs
{
    public record CreateCategoryDto(
        string Name,
        string? Description,
        int? ParentCategoryId,
        bool Status
    );

    public record UpdateCategoryDto(
        string Name,
        string? Description,
        int? ParentCategoryId,
        bool Status
    );

    public record CategoryDto(
        int Id,
        string Name,
        string? Description,
        bool Status,
        int? ParentCategoryId,
        List<CategoryDto>? SubCategories
    );

    public record CategoryQueryParameters(
        string? SearchQuery = null,
        bool? Status = null,
        int PageIndex = 1,
        int PageSize = 10
    );
}
