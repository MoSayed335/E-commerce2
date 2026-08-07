using ECommerce2.Repositories.Interfaces;
using ECommerce2.Models;
using ECommerce2.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ECommerce2.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<ECommerce2.Utilities.PaginatedList<Category>> GetAllForAdminAsync(ECommerce2.DTOs.CategoryQueryParameters parameters)
        {
            var query = DbSet.Include(c => c.ParentCategory).AsNoTracking();

            if (parameters.Status.HasValue)
            {
                query = query.Where(c => c.Status == parameters.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var lowerSearch = parameters.SearchQuery.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(lowerSearch));
            }

            var orderedQuery = query.OrderByDescending(c => c.CreatedAt);
            return await ECommerce2.Utilities.PaginatedList<Category>.CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);
        }

        public async Task<List<Category>> GetActiveCategoriesWithSubcategoriesAsync()
        {
            return await DbSet.Include(c => c.SubCategories)
                              .Where(c => c.Status && c.ParentCategoryId == null)
                              .OrderBy(c => c.Name)
                              .AsNoTracking()
                              .ToListAsync();
        }
    }

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

        public async Task<ECommerce2.Utilities.PaginatedList<Product>> GetAllForAdminAsync(ECommerce2.DTOs.ProductQueryParameters parameters)
        {
            var query = DbSet
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .AsNoTracking();

            if (parameters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);
            }
            
            if (parameters.Status.HasValue)
            {
                query = query.Where(p => p.Status == parameters.Status.Value);
            }
            
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var lowerSearch = parameters.SearchQuery.ToLower();
                query = query.Where(p => 
                    p.Name.ToLower().Contains(lowerSearch) || 
                    p.Sku.ToLower().Contains(lowerSearch));
            }

            var orderedQuery = query.OrderByDescending(p => p.CreatedAt);
            
            return await ECommerce2.Utilities.PaginatedList<Product>.CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);
        }
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
                .Include(o => o.Governorate)
                .FirstOrDefaultAsync(o => o.Id == orderId);

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(string userId) =>
            await DbSet
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product)
                .Include(o => o.Governorate)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        public async Task<string> GenerateNextOrderNumberAsync()
        {
            var count = await DbSet.CountAsync();
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{count + 1:D4}";
        }

        public async Task<ECommerce2.Utilities.PaginatedList<Order>> GetAllForAdminAsync(ECommerce2.DTOs.OrderQueryParameters parameters)
        {
            var query = DbSet
                .Include(o => o.User)
                .Include(o => o.Governorate)
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product)
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Color)
                .Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Size)
                .AsNoTracking();

            if (parameters.Status.HasValue)
            {
                query = query.Where(o => o.Status == parameters.Status.Value);
            }
            
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var lowerSearch = parameters.SearchQuery.ToLower();
                query = query.Where(o => 
                    o.OrderNumber.ToLower().Contains(lowerSearch) || 
                    (o.User.FName + " " + o.User.LName).ToLower().Contains(lowerSearch) || 
                    o.User.PhoneNumber.Contains(lowerSearch));
            }

            var orderedQuery = query.OrderByDescending(o => o.CreatedAt);
            
            return await ECommerce2.Utilities.PaginatedList<Order>.CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);
        }
    }

    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext context) : base(context) { }

        public async Task<Coupon?> GetByCodeAsync(string code) =>
            await DbSet.FirstOrDefaultAsync(c => c.Code == code);

        public async Task<ECommerce2.Utilities.PaginatedList<Coupon>> GetAllForAdminAsync(ECommerce2.DTOs.CouponQueryParameters parameters)
        {
            var query = DbSet.AsNoTracking();

            if (parameters.Status.HasValue)
            {
                query = query.Where(c => c.Status == parameters.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var lowerSearch = parameters.SearchQuery.ToLower();
                query = query.Where(c => 
                    c.Code.ToLower().Contains(lowerSearch) || 
                    (c.Name != null && c.Name.ToLower().Contains(lowerSearch)));
            }

            var orderedQuery = query.OrderByDescending(c => c.CreatedAt);
            
            return await ECommerce2.Utilities.PaginatedList<Coupon>.CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);
        }

        public async Task<List<ECommerce2.DTOs.CampaignTrackingDto>> GetCampaignTrackingStatsAsync()
        {
            var groups = await DbSet
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .GroupBy(c => c.Name)
                .Select(g => new { Name = g.Key!, Redemptions = g.Sum(c => c.TimesUsed) })
                .OrderByDescending(x => x.Redemptions)
                .ToListAsync();

            return groups.Select(x => new ECommerce2.DTOs.CampaignTrackingDto(x.Name, x.Redemptions)).ToList();
        }
    }

    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public async Task<Cart?> GetByUserIdWithItemsAsync(string userId) =>
            await DbSet
                .Include(c => c.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product).ThenInclude(p => p.Images)
                .Include(c => c.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Color)
                .Include(c => c.Items).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Size)
                .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public class GovernorateRepository : GenericRepository<Governorate>, IGovernorateRepository
    {
        public GovernorateRepository(AppDbContext context) : base(context) { }
    }

    public class StoreSettingRepository : GenericRepository<StoreSetting>, IStoreSettingRepository
    {
        public StoreSettingRepository(AppDbContext context) : base(context) { }

        public async Task<StoreSetting?> GetByKeyAsync(string key) =>
            await DbSet.FirstOrDefaultAsync(s => s.Key == key);
    }

    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<ECommerce2.Utilities.PaginatedList<Review>> GetAllForAdminAsync(ECommerce2.DTOs.ReviewQueryParameters parameters)
        {
            var query = DbSet
                .Include(r => r.User)
                .Include(r => r.Product)
                .AsNoTracking();

            if (parameters.Status.HasValue)
            {
                query = query.Where(r => r.Status == parameters.Status.Value);
            }

            if (parameters.IsPinned.HasValue)
            {
                query = query.Where(r => r.IsPinned == parameters.IsPinned.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var lowerSearch = parameters.SearchQuery.ToLower();
                query = query.Where(r => 
                    (r.User.FName + " " + r.User.LName).ToLower().Contains(lowerSearch) ||
                    r.Product.Name.ToLower().Contains(lowerSearch) ||
                    r.Comment.ToLower().Contains(lowerSearch));
            }

            // Order by Pinned first, then by Creation date descending
            var orderedQuery = query
                .OrderByDescending(r => r.IsPinned)
                .ThenByDescending(r => r.CreatedAt);
            
            return await ECommerce2.Utilities.PaginatedList<Review>.CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);
        }

        public async Task<ECommerce2.Utilities.PaginatedList<Review>> GetApprovedReviewsForProductAsync(int productId, int pageNumber, int pageSize)
        {
            var query = DbSet.AsNoTracking()
                             .Include(r => r.User)
                             .Where(r => r.Product.Id == productId && r.Status == Models.Enums.ReviewStatus.Approved)
                             .OrderByDescending(r => r.CreatedAt);

            return await ECommerce2.Utilities.PaginatedList<Review>.CreateAsync(query, pageNumber, pageSize);
        }
    }

    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context) { }

        public async Task<List<Favorite>> GetUserFavoritesAsync(string userId)
        {
            return await DbSet.AsNoTracking()
                              .Include(f => f.Product)
                              .ThenInclude(p => p.Images)
                              .Where(f => f.UserId == userId)
                              .OrderByDescending(f => f.CreatedAt)
                              .ToListAsync();
        }

        public async Task<Favorite?> GetUserFavoriteAsync(string userId, int productId)
        {
            return await DbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }

    public class UserAddressRepository : GenericRepository<UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(AppDbContext context) : base(context) { }

        public async Task<List<UserAddress>> GetUserAddressesAsync(string userId)
        {
            return await DbSet.Include(a => a.Governorate)
                              .Where(a => a.UserId == userId)
                              .AsNoTracking()
                              .ToListAsync();
        }
    }

    public class BannerRepository : GenericRepository<Banner>, IBannerRepository
    {
        public BannerRepository(AppDbContext context) : base(context) { }

        public async Task<ECommerce2.Utilities.PaginatedList<Banner>> GetAllForAdminAsync(ECommerce2.DTOs.BannerQueryParameters parameters)
        {
            var query = DbSet
                .Include(b => b.BannerProducts)
                .Include(b => b.BannerCategories)
                .AsNoTracking();

            if (parameters.Status.HasValue)
                query = query.Where(b => b.Status == parameters.Status.Value);

            if (parameters.Type.HasValue)
                query = query.Where(b => b.Type == parameters.Type.Value);

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                var lowerSearch = parameters.SearchQuery.ToLower();
                query = query.Where(b => b.Name.ToLower().Contains(lowerSearch));
            }

            var orderedQuery = query.OrderByDescending(b => b.CreatedAt);
            return await ECommerce2.Utilities.PaginatedList<Banner>.CreateAsync(orderedQuery, parameters.PageIndex, parameters.PageSize);
        }

        public async Task<List<Banner>> GetActiveBannersAsync()
        {
            var now = DateTime.UtcNow;
            return await DbSet
                .Include(b => b.BannerProducts)
                .Include(b => b.BannerCategories)
                .Where(b => b.Status && b.StartAt <= now && (!b.EndAt.HasValue || b.EndAt > now))
                .OrderByDescending(b => b.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }
    }

    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(AppDbContext context) : base(context) { }

        public async Task<List<Color>> GetAllColorsAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }
    }

    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(AppDbContext context) : base(context) { }

        public async Task<List<Size>> GetAllSizesAsync()
        {
            return await DbSet.OrderBy(s => s.SortOrder).AsNoTracking().ToListAsync();
        }
    }
}
