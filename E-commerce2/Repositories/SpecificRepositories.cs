using ECommerce2.Repositories.Interfaces;
using ECommerce2.Models;
using ECommerce2.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ECommerce2.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<Product?> GetWithVariantsAsync(int productId) =>
            await DbSet
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants).ThenInclude(v => v.Color)
                .Include(p => p.Variants).ThenInclude(v => v.Size)
                .FirstOrDefaultAsync(p => p.Id == productId);

        public async Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId) =>
            await DbSet.Where(p => p.CategoryId == categoryId && p.Status).ToListAsync();
    }

    public class ProductVariantRepository : GenericRepository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(AppDbContext context) : base(context) { }

        // في الإنتاج الفعلي هنا محتاج قفل تشاؤمي/تفاؤلي (Pessimistic/Optimistic Locking)
        // على صف الـ Stock عشان تمنع Race Condition لو أكتر من عميل بيشتري آخر قطعة في نفس اللحظة
        public async Task<ProductVariant?> GetByIdWithStockLockAsync(int variantId) =>
            await DbSet
                .Include(v => v.Product)
                .Include(v => v.Color)
                .Include(v => v.Size)
                .FirstOrDefaultAsync(v => v.Id == variantId);
    }

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<Order?> GetWithItemsAsync(int orderId) =>
            await DbSet
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product)
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Color)
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Size)
                .FirstOrDefaultAsync(o => o.Id == orderId);

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(string userId) =>
            await DbSet
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        public async Task<string> GenerateNextOrderNumberAsync()
        {
            var count = await DbSet.CountAsync();
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{count + 1:D4}";
        }
    }

    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext context) : base(context) { }

        public async Task<Coupon?> GetByCodeAsync(string code) =>
            await DbSet.FirstOrDefaultAsync(c => c.Code == code);
    }

    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public async Task<Cart?> GetByUserIdWithItemsAsync(string userId) =>
            await DbSet
                .Include(c => c.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}
