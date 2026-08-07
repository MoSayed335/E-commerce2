using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ICartRepository _cartRepository;

        public CouponService(ICouponRepository couponRepository, ICartRepository cartRepository)
        {
            _couponRepository = couponRepository;
            _cartRepository = cartRepository;
        }

        public async Task<Result<decimal>> ValidateCouponAsync(string code, string userId)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null || !coupon.Status)
                return Result<decimal>.Failure("الكوبون غير صالح.");

            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate < DateTime.UtcNow)
                return Result<decimal>.Failure("الكوبون منتهي الصلاحية.");

            if (coupon.UsageLimit.HasValue && coupon.TimesUsed >= coupon.UsageLimit)
                return Result<decimal>.Failure("تم استنفاذ عدد مرات استخدام الكوبون.");

            // Get user's cart to check subtotal
            var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId);
            if (cart == null || !cart.Items.Any())
                return Result<decimal>.Failure("عربة التسوق فارغة.");

            decimal subTotal = 0;
            foreach(var item in cart.Items)
            {
                var unitPrice = item.ProductVariant?.PriceOverride ?? item.ProductVariant?.Product?.Price ?? 0;
                subTotal += unitPrice * item.Quantity;
            }

            if (coupon.MinOrderAmount.HasValue && subTotal < coupon.MinOrderAmount)
                return Result<decimal>.Failure($"الحد الأدنى لاستخدام الكوبون هو {coupon.MinOrderAmount}.");

            var discount = coupon.DiscountType == Models.Enums.DiscountType.Percentage
                ? subTotal * (coupon.Value / 100)
                : coupon.Value;

            if (coupon.MaxDiscountAmount.HasValue)
                discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);

            return Result<decimal>.Success(discount);
        }
    }
}
