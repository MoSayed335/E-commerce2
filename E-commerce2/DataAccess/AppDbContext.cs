using ECommerce2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce2.DataAccess
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {

        }
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAddress> UserAddresses => Set<UserAddress>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<Size> Sizes => Set<Size>();

        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<CouponProduct> CouponProducts => Set<CouponProduct>();
        public DbSet<CouponCategory> CouponCategories => Set<CouponCategory>();

        public DbSet<Banner> Banners => Set<Banner>();
        public DbSet<BannerProduct> BannerProducts => Set<BannerProduct>();
        public DbSet<BannerCategory> BannerCategories => Set<BannerCategory>();

        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Governorate> Governorates => Set<Governorate>();
        public DbSet<StoreSetting> StoreSettings => Set<StoreSetting>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
