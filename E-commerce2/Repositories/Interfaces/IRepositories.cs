using ECommerce2.Utilities;
using ECommerce2.Models;
using System.Text;

namespace ECommerce2.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<PaginatedList<Category>> GetAllForAdminAsync(ECommerce2.DTOs.CategoryQueryParameters parameters);
        Task<List<Category>> GetActiveCategoriesWithSubcategoriesAsync();
    }

    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetWithVariantsAsync(int productId);
        Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId);
        Task<PaginatedList<Product>> GetAllForAdminAsync(ECommerce2.DTOs.ProductQueryParameters parameters);
    }

    public interface IProductVariantRepository : IGenericRepository<ProductVariant>
    {
        Task<ProductVariant?> GetByIdWithStockLockAsync(int variantId);
    }

    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetWithItemsAsync(int orderId);
        Task<IReadOnlyList<Order>> GetByUserIdAsync(string userId);
        Task<string> GenerateNextOrderNumberAsync();
        Task<PaginatedList<Order>> GetAllForAdminAsync(ECommerce2.DTOs.OrderQueryParameters parameters);
    }

    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon?> GetByCodeAsync(string code);
        Task<PaginatedList<Coupon>> GetAllForAdminAsync(ECommerce2.DTOs.CouponQueryParameters parameters);
        Task<List<ECommerce2.DTOs.CampaignTrackingDto>> GetCampaignTrackingStatsAsync();
    }

    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetByUserIdWithItemsAsync(string userId);
    }

    public interface IGovernorateRepository : IGenericRepository<Governorate>
    {
    }

    public interface IStoreSettingRepository : IGenericRepository<StoreSetting>
    {
        Task<StoreSetting?> GetByKeyAsync(string key);
    }

    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<PaginatedList<Review>> GetAllForAdminAsync(ECommerce2.DTOs.ReviewQueryParameters parameters);
        Task<PaginatedList<Review>> GetApprovedReviewsForProductAsync(int productId, int pageNumber, int pageSize);
    }

    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        Task<List<Favorite>> GetUserFavoritesAsync(string userId);
        Task<Favorite?> GetUserFavoriteAsync(string userId, int productId);
    }

    public interface IUserAddressRepository : IGenericRepository<UserAddress>
    {
        Task<List<UserAddress>> GetUserAddressesAsync(string userId);
    }

    public interface IBannerRepository : IGenericRepository<Banner>
    {
        Task<PaginatedList<Banner>> GetAllForAdminAsync(ECommerce2.DTOs.BannerQueryParameters parameters);
        Task<List<Banner>> GetActiveBannersAsync();
    }

    public interface IColorRepository : IGenericRepository<Color>
    {
        Task<List<Color>> GetAllColorsAsync();
    }

    public interface ISizeRepository : IGenericRepository<Size>
    {
        Task<List<Size>> GetAllSizesAsync();
    }
}
