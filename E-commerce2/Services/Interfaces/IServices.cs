using ECommerce2.Utilities;
using ECommerce2.DTOs;
using ECommerce2.DTOs.Responses;

namespace ECommerce2.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<ProductListItemDto>> GetByCategoryAsync(int categoryId);
        Task<Result<int>> CreateAsync(CreateProductDto dto);
        Task<PaginatedList<AdminProductSummaryDto>> GetAdminProductsAsync(ProductQueryParameters parameters);
        Task<Result> UpdateAsync(int id, UpdateProductDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateStatusAsync(int id, bool status);
    }

    public interface ICategoryService
    {
        Task<Result<int>> CreateAsync(CreateCategoryDto dto);
        Task<Result> UpdateAsync(int id, UpdateCategoryDto dto);
        Task<Result> DeleteAsync(int id);
        Task<PaginatedList<CategoryDto>> GetAdminCategoriesAsync(CategoryQueryParameters parameters);
        Task<List<CategoryDto>> GetStorefrontCategoriesAsync();
    }

    public interface IOrderService
    {
        Task<Result<OrderDetailsDto>> CreateOrderAsync(string userId, CreateOrderDto dto);
        Task<OrderDetailsDto?> GetByIdAsync(int orderId, string? userId = null);
        Task<IReadOnlyList<OrderDetailsDto>> GetByUserIdAsync(string userId);
        Task<Result> UpdateStatusAsync(int orderId, Models.Enums.OrderStatus newStatus);
        Task<PaginatedList<AdminOrderSummaryDto>> GetAdminOrdersAsync(OrderQueryParameters parameters);
        Task<Result> CancelOrderAsync(int orderId);
        Task<Result> UpdateAdminNotesAsync(int orderId, string? notes);
        Task<Result<OrderTrackerDto>> TrackOrderAsync(string orderNumber, string email);
        Task<Result<OrderDetailsDto>> CheckoutAsync(string userId, CheckoutDto dto);
    }

    public interface ICouponService
    {
        Task<Result<decimal>> ValidateCouponAsync(string code, string userId);
    }

    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string userId);
        Task<Result> AddItemAsync(string userId, AddToCartDto dto);
        Task<Result> UpdateItemQuantityAsync(string userId, int cartItemId, int quantity);
        Task<Result> RemoveItemAsync(string userId, int cartItemId);
    }

    public interface IShippingService
    {
        Task<ShippingSettingsDto> GetSettingsAsync();
        Task<Result> UpdateFreeShippingThresholdAsync(decimal threshold);
        Task<Result> UpdateGovernoratesAsync(List<UpdateGovernorateDto> governorates);
        Task<Result> CreateGovernorateAsync(CreateGovernorateDto dto);
        Task<Result> DeleteGovernorateAsync(int id);
    }

    public interface ICouponAdminService
    {
        Task<Result<int>> CreateAsync(CreateCouponDto dto);
        Task<PaginatedList<AdminCouponSummaryDto>> GetAdminCouponsAsync(CouponQueryParameters parameters);
        Task<Result> UpdateStatusAsync(int id, bool status);
        Task<Result> DeleteAsync(int id);
        Task<List<CampaignTrackingDto>> GetCampaignTrackingStatsAsync();
    }

    public interface IReviewAdminService
    {
        Task<PaginatedList<AdminReviewSummaryDto>> GetAdminReviewsAsync(ReviewQueryParameters parameters);
        Task<Result> UpdateStatusAsync(int id, Models.Enums.ReviewStatus status);
        Task<Result> TogglePinAsync(int id);
        Task<int> GetLiveUrgencyCounterAsync();
        Task<Result> UpdateLiveUrgencyCounterAsync(int baseCounter);
    }

    public interface IFavoriteService
    {
        Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId);
        Task<Result> AddToFavoritesAsync(string userId, int productId);
        Task<Result> RemoveFromFavoritesAsync(string userId, int productId);
    }

    public interface ICustomerAdminService
    {
        Task<PaginatedList<AdminCustomerSummaryDto>> GetAdminCustomersAsync(int pageIndex, int pageSize, string? searchQuery);
    }

    public interface IReviewService
    {
        Task<PaginatedList<StorefrontReviewDto>> GetProductReviewsAsync(int productId, int pageIndex, int pageSize);
        Task<Result> AddReviewAsync(string userId, CreateReviewDto dto);
    }

    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    }

    public interface IProfileService
    {
        Task<Result<UserProfileDto>> GetProfileAsync(string userId);
        Task<Result<int>> AddAddressAsync(string userId, CreateUserAddressDto dto);
        Task<Result> UpdateAddressAsync(string userId, int addressId, UpdateUserAddressDto dto);
        Task<Result> DeleteAddressAsync(string userId, int addressId);
    }

    public interface IBannerService
    {
        Task<Result<int>> CreateAsync(CreateBannerDto dto);
        Task<Result> UpdateAsync(int id, UpdateBannerDto dto);
        Task<Result> DeleteAsync(int id);
        Task<PaginatedList<BannerDto>> GetAdminBannersAsync(BannerQueryParameters parameters);
        Task<List<BannerDto>> GetStorefrontBannersAsync();
    }

    public interface IAttributeService
    {
        Task<List<ColorDto>> GetColorsAsync();
        Task<Result<int>> CreateColorAsync(CreateColorDto dto);
        Task<Result> UpdateColorAsync(int id, UpdateColorDto dto);
        Task<Result> DeleteColorAsync(int id);

        Task<List<SizeDto>> GetSizesAsync();
        Task<Result<int>> CreateSizeAsync(CreateSizeDto dto);
        Task<Result> UpdateSizeAsync(int id, UpdateSizeDto dto);
        Task<Result> DeleteSizeAsync(int id);
    }

    public interface IFileService
    {
        Task<Result<string>> UploadFileAsync(Microsoft.AspNetCore.Http.IFormFile file, string subFolder = "");
    }
}
