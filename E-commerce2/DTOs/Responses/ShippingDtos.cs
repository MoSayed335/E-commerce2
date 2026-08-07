namespace ECommerce2.DTOs
{
    public record GovernorateDto(
        int Id,
        string NameEn,
        string NameAr,
        decimal Fee,
        string EstimatedDelivery
    );

    public record UpdateGovernorateDto(
        int Id,
        decimal Fee,
        string EstimatedDelivery
    );

    public record UpdateShippingRatesRequest(
        List<UpdateGovernorateDto> Governorates
    );

    public record UpdateFreeShippingThresholdRequest(
        decimal Threshold
    );

    public record ShippingSettingsDto(
        decimal FreeShippingThreshold,
        List<GovernorateDto> Governorates
    );

    public record CreateGovernorateDto(
        string NameEn,
        string NameAr,
        decimal Fee,
        string EstimatedDelivery
    );
}
