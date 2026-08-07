using ECommerce2.Utilities;
using ECommerce2.Models;
using System.Text;

namespace ECommerce2.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetWithVariantsAsync(int productId);
        Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId);
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
    }

    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon?> GetByCodeAsync(string code);
    }

    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetByUserIdWithItemsAsync(string userId);
    }
}
