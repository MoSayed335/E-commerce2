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

        public async Task<Result<OrderDetailsDto>> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            if (dto.Items is null || dto.Items.Count == 0)
                return Result<OrderDetailsDto>.Failure("لا يمكن إنشاء طلب بدون أصناف.");

            var order = new Order
            {
                UserId = userId,
                OrderNumber = await _orderRepository.GenerateNextOrderNumberAsync(),
                Status = OrderStatus.Pending,
                Notes = dto.Notes,
                // ملحوظة: هنا المفروض تجيب العنوان الفعلي من UserAddressId
                // مختصرها هنا للتوضيح فقط
                ShippingAddress = "Resolved from UserAddressId",
                GovernorateId = 1 // Placeholder until real address logic is implemented
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
            order.DeliveryFee = CalculateDeliveryFee(order.GovernorateId);
            order.TotalPrice = order.SubTotal - order.DiscountAmount + order.DeliveryFee;

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(); // كل التغييرات (Order + Stock) في Transaction واحدة

            return Result<OrderDetailsDto>.Success(MapToDto(order));
        }

        public async Task<Result<OrderDetailsDto>> CheckoutAsync(string userId, CheckoutDto dto)
        {
            var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId);
            if (cart == null || !cart.Items.Any())
                return Result<OrderDetailsDto>.Failure("عربة التسوق فارغة.");

            // In a real app, resolve real address logic here.
            var order = new Order
            {
                UserId = userId,
                OrderNumber = await _orderRepository.GenerateNextOrderNumberAsync(),
                Status = OrderStatus.Pending,
                Notes = dto.Notes,
                ShippingAddress = "Resolved from UserAddressId",
                GovernorateId = 1
            };

            decimal subTotal = 0;

            foreach (var item in cart.Items)
            {
                var variant = await _variantRepository.GetByIdWithStockLockAsync(item.ProductVariantId);
                if (variant == null || variant.Stock < item.Quantity)
                    return Result<OrderDetailsDto>.Failure($"الكمية غير متوفرة للمنتج (SKU: {variant?.Sku}).");

                var unitPrice = variant.PriceOverride ?? variant.Product.Price;

                order.Items.Add(new OrderItem
                {
                    ProductVariantId = variant.Id,
                    Quantity = item.Quantity,
                    UnitPriceAtPurchase = unitPrice
                });

                subTotal += unitPrice * item.Quantity;
                variant.Stock -= item.Quantity;
            }

            order.SubTotal = subTotal;

            decimal discount = 0;
            if (!string.IsNullOrWhiteSpace(dto.CouponCode))
            {
                var couponResult = await ApplyCouponAsync(dto.CouponCode, subTotal);
                if (!couponResult.IsSuccess)
                    return Result<OrderDetailsDto>.Failure(couponResult.Error!);

                discount = couponResult.Value;
            }

            order.DiscountAmount = discount;
            order.DeliveryFee = CalculateDeliveryFee(order.GovernorateId);
            order.TotalPrice = order.SubTotal - order.DiscountAmount + order.DeliveryFee;

            await _orderRepository.AddAsync(order);
            
            // Clear cart
            foreach (var cartItem in cart.Items.ToList())
            {
                cart.Items.Remove(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result<OrderDetailsDto>.Success(MapToDto(order));
        }

        public async Task<OrderDetailsDto?> GetByIdAsync(int orderId, string? userId = null)
        {
            var order = await _orderRepository.GetWithItemsAsync(orderId);
            if (order is null) return null;
            if (userId != null && order.UserId != userId) return null; // IDOR Protection
            return MapToDto(order);
        }

        public async Task<IReadOnlyList<OrderDetailsDto>> GetByUserIdAsync(string userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        public async Task<PaginatedList<AdminOrderSummaryDto>> GetAdminOrdersAsync(OrderQueryParameters parameters)
        {
            var orders = await _orderRepository.GetAllForAdminAsync(parameters);
            var summaryList = orders.Items.Select(order => new AdminOrderSummaryDto(
                order.Id,
                order.OrderNumber,
                order.Status.ToString(),
                $"{order.User?.FName} {order.User?.LName}".Trim(),
                order.User?.PhoneNumber ?? string.Empty,
                order.Governorate?.NameAr ?? string.Empty,
                order.ShippingAddress,
                order.TotalPrice,
                order.CreatedAt,
                order.AdminNotes,
                order.Items.Select(i => new OrderItemDto(
                    i.ProductVariant?.Product?.Name ?? string.Empty,
                    i.ProductVariant?.Color?.Name,
                    i.ProductVariant?.Size?.Name,
                    i.Quantity,
                    i.UnitPriceAtPurchase,
                    i.Subtotal
                )).ToList()
            )).ToList();

            return new PaginatedList<AdminOrderSummaryDto>(summaryList, orders.TotalCount, orders.PageIndex, parameters.PageSize);
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

        public async Task<Result> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetWithItemsAsync(orderId);
            if (order is null)
                return Result.Failure("الطلب غير موجود.");

            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
                return Result.Failure("لا يمكن تعديل حالة طلب مكتمل أو ملغي.");

            order.Status = OrderStatus.Cancelled;

            // Revert stock
            foreach (var item in order.Items)
            {
                var variant = await _variantRepository.GetByIdWithStockLockAsync(item.ProductVariantId);
                if (variant != null)
                {
                    variant.Stock += item.Quantity;
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateAdminNotesAsync(int orderId, string? notes)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                return Result.Failure("الطلب غير موجود.");

            order.AdminNotes = notes;
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
        private decimal CalculateDeliveryFee(int governorateId) => 30m;

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
            order.AdminNotes,
            order.Items.Select(i => new OrderItemDto(
                i.ProductVariant?.Product?.Name ?? string.Empty,
                i.ProductVariant?.Color?.Name,
                i.ProductVariant?.Size?.Name,
                i.Quantity,
                i.UnitPriceAtPurchase,
                i.Subtotal
            )).ToList()
        );

        public async Task<Result<OrderTrackerDto>> TrackOrderAsync(string orderNumber, string email)
        {
            var order = await _orderRepository.FindAsync(o => o.OrderNumber == orderNumber && o.User.Email == email);
            var targetOrder = order.FirstOrDefault();
            
            if (targetOrder == null)
                return Result<OrderTrackerDto>.Failure("الطلب غير موجود أو البريد الإلكتروني غير متطابق.");

            var stages = new List<OrderTrackingStage>
            {
                new OrderTrackingStage("Pending", "الطلب قيد المراجعة", targetOrder.Status >= OrderStatus.Pending, targetOrder.CreatedAt),
                new OrderTrackingStage("Processing", "جاري تجهيز الطلب", targetOrder.Status >= OrderStatus.Processing, targetOrder.Status >= OrderStatus.Processing ? targetOrder.CreatedAt.AddHours(2) : null), // Simulated time for demo
                new OrderTrackingStage("Shipped", "تم شحن الطلب", targetOrder.Status >= OrderStatus.Shipped, targetOrder.Status >= OrderStatus.Shipped ? targetOrder.CreatedAt.AddHours(24) : null),
                new OrderTrackingStage("Delivered", "تم توصيل الطلب", targetOrder.Status == OrderStatus.Delivered, targetOrder.DeliveredAt)
            };

            if (targetOrder.Status == OrderStatus.Cancelled)
            {
                stages.Clear();
                stages.Add(new OrderTrackingStage("Cancelled", "تم إلغاء الطلب", true, targetOrder.CreatedAt));
            }

            var tracker = new OrderTrackerDto(
                targetOrder.OrderNumber,
                targetOrder.Status.ToString(),
                targetOrder.TotalPrice,
                targetOrder.ShippingAddress,
                stages
            );

            return Result<OrderTrackerDto>.Success(tracker);
        }
    }
}
