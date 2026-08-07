using ECommerce2.Utilities;
using ECommerce2.DTOs;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Models;
using ECommerce2.Models.Enums;

namespace ECommerce2.Services
{
   
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductVariantRepository _variantRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        // Constructor Injection - كل الاعتماديات Interfaces، تقدر تستبدل أي واحدة فيهم
        // (مثلاً في الاختبارات) من غير ما تعدّل الكلاس ده (OCP + DIP)
        public OrderService(
            IOrderRepository orderRepository,
            IProductVariantRepository variantRepository,
            ICouponRepository couponRepository,
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _variantRepository = variantRepository;
            _couponRepository = couponRepository;
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OrderDetailsDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            if (dto.Items is null || dto.Items.Count == 0)
                return Result<OrderDetailsDto>.Failure("لا يمكن إنشاء طلب بدون أصناف.");

            var order = new Order
            {
                UserId = dto.UserId,
                OrderNumber = await _orderRepository.GenerateNextOrderNumberAsync(),
                Status = OrderStatus.Pending,
                Notes = dto.Notes,
                // ملحوظة: هنا المفروض تجيب العنوان الفعلي من UserAddressId
                // مختصرها هنا للتوضيح فقط
                ShippingAddress = "Resolved from UserAddressId",
                ShippingCity = "Resolved from UserAddressId"
            };

            decimal subTotal = 0;

            foreach (var itemDto in dto.Items)
            {
                var variant = await _variantRepository.GetByIdWithStockLockAsync(itemDto.ProductVariantId);
                if (variant is null)
                    return Result<OrderDetailsDto>.Failure($"المنتج غير موجود (VariantId: {itemDto.ProductVariantId}).");

                if (variant.Stock < itemDto.Quantity)
                    return Result<OrderDetailsDto>.Failure($"الكمية المتاحة غير كافية للمنتج (SKU: {variant.Sku}).");

                // Snapshot السعر وقت الشراء - مش السعر الحالي وقت العرض لاحقًا
                var unitPrice = variant.PriceOverride ?? variant.Product.Price;

                order.Items.Add(new OrderItem
                {
                    ProductVariantId = variant.Id,
                    Quantity = itemDto.Quantity,
                    UnitPriceAtPurchase = unitPrice
                });

                subTotal += unitPrice * itemDto.Quantity;
                variant.Stock -= itemDto.Quantity; // خصم المخزون فورًا
            }

            order.SubTotal = subTotal;

            // تطبيق الكوبون لو موجود
            decimal discount = 0;
            if (!string.IsNullOrWhiteSpace(dto.CouponCode))
            {
                var couponResult = await ApplyCouponAsync(dto.CouponCode, subTotal);
                if (!couponResult.IsSuccess)
                    return Result<OrderDetailsDto>.Failure(couponResult.Error!);

                discount = couponResult.Value;
            }

            order.DiscountAmount = discount;
            order.DeliveryFee = CalculateDeliveryFee(order.ShippingCity);
            order.TotalPrice = order.SubTotal - order.DiscountAmount + order.DeliveryFee;

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(); // كل التغييرات (Order + Stock) في Transaction واحدة

            return Result<OrderDetailsDto>.Success(MapToDto(order));
        }

        public async Task<OrderDetailsDto?> GetByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetWithItemsAsync(orderId);
            return order is null ? null : MapToDto(order);
        }

        public async Task<IReadOnlyList<OrderDetailsDto>> GetByUserIdAsync(string userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        public async Task<Result> UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                return Result.Failure("الطلب غير موجود.");

            // منطق انتقال الحالات - يمنع مثلاً الرجوع من Delivered لـ Pending
            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
                return Result.Failure("لا يمكن تعديل حالة طلب مكتمل أو ملغي.");

            order.Status = newStatus;
            if (newStatus == OrderStatus.Delivered)
                order.DeliveredAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        private async Task<Result<decimal>> ApplyCouponAsync(string code, decimal subTotal)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon is null || !coupon.Status)
                return Result<decimal>.Failure("الكوبون غير صالح.");

            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate < DateTime.UtcNow)
                return Result<decimal>.Failure("الكوبون منتهي الصلاحية.");

            if (coupon.UsageLimit.HasValue && coupon.TimesUsed >= coupon.UsageLimit)
                return Result<decimal>.Failure("تم استنفاذ عدد مرات استخدام الكوبون.");

            if (coupon.MinOrderAmount.HasValue && subTotal < coupon.MinOrderAmount)
                return Result<decimal>.Failure($"الحد الأدنى لاستخدام الكوبون هو {coupon.MinOrderAmount}.");

            var discount = coupon.DiscountType == DiscountType.Percentage
                ? subTotal * (coupon.Value / 100)
                : coupon.Value;

            if (coupon.MaxDiscountAmount.HasValue)
                discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);

            coupon.TimesUsed++;
            return Result<decimal>.Success(discount);
        }

        // مثال بسيط - في الواقع ممكن يعتمد على وزن الطلب أو منطقة الشحن
        private decimal CalculateDeliveryFee(string city) => 30m;

        private static OrderDetailsDto MapToDto(Order order) => new(
            order.Id,
            order.OrderNumber,
            order.Status.ToString(),
            order.SubTotal,
            order.DiscountAmount,
            order.DeliveryFee,
            order.TotalPrice,
            order.ShippingAddress,
            order.CreatedAt,
            order.Items.Select(i => new OrderItemDto(
                i.ProductVariant?.Product?.Name ?? string.Empty,
                i.ProductVariant?.Color?.Name,
                i.ProductVariant?.Size?.Name,
                i.Quantity,
                i.UnitPriceAtPurchase,
                i.Subtotal
            )).ToList()
        );
    }
}
