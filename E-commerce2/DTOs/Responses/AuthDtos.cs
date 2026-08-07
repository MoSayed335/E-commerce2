namespace ECommerce2.DTOs
{
    public record RegisterDto(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber
    );

    public record LoginDto(
        string Email,
        string Password
    );

    public record AuthResponseDto(
        string Token,
        string Email,
        string FirstName,
        string LastName,
        List<string> Roles
    );
}
