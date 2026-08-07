namespace ECommerce2.DTOs
{
    public record UserProfileDto(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTime CreatedAt,
        List<UserAddressDto> Addresses
    );

    public record UserAddressDto(
        int Id,
        string FullAddress,
        string? Landmark,
        int GovernorateId,
        string GovernorateName,
        bool IsDefault
    );

    public record CreateUserAddressDto(
        string FullAddress,
        string? Landmark,
        int GovernorateId,
        bool IsDefault
    );

    public record UpdateUserAddressDto(
        string FullAddress,
        string? Landmark,
        int GovernorateId,
        bool IsDefault
    );
}
