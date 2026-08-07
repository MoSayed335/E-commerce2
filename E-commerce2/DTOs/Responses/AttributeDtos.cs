namespace ECommerce2.DTOs
{
    public record CreateColorDto(
        string Name,
        string HexCode
    );

    public record UpdateColorDto(
        string Name,
        string HexCode
    );

    public record ColorDto(
        int Id,
        string Name,
        string HexCode
    );

    public record CreateSizeDto(
        string Name,
        int SortOrder
    );

    public record UpdateSizeDto(
        string Name,
        int SortOrder
    );

    public record SizeDto(
        int Id,
        string Name,
        int SortOrder
    );
}
