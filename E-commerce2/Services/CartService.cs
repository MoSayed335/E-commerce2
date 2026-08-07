using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductVariantRepository _variantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(ICartRepository cartRepository, IProductVariantRepository variantRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _variantRepository = variantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId);
            if (cart == null)
            {
                return new CartDto(0, 0, new List<CartItemDto>());
            }

            return MapToDto(cart);
        }

        public async Task<Result> AddItemAsync(string userId, AddToCartDto dto)
        {
            var variant = await _variantRepository.GetByIdAsync(dto.ProductVariantId);
            if (variant == null) return Result.Failure("Product variant not found.");
            if (variant.Stock < dto.Quantity) return Result.Failure("Insufficient stock.");

            var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == dto.ProductVariantId);
            if (existingItem != null)
            {
                if (variant.Stock < existingItem.Quantity + dto.Quantity)
                    return Result.Failure("Insufficient stock for requested quantity.");
                
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            if (quantity <= 0) return Result.Failure("Quantity must be greater than zero.");

            var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId);
            if (cart == null) return Result.Failure("Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item == null) return Result.Failure("Item not found in cart.");

            var variant = await _variantRepository.GetByIdAsync(item.ProductVariantId);
            if (variant == null) return Result.Failure("Product variant not found.");

            if (variant.Stock < quantity) return Result.Failure("Insufficient stock.");

            item.Quantity = quantity;
            await _unitOfWork.SaveChangesAsync();
            
            return Result.Success();
        }

        public async Task<Result> RemoveItemAsync(string userId, int cartItemId)
        {
            var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId);
            if (cart == null) return Result.Failure("Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item == null) return Result.Failure("Item not found in cart.");

            cart.Items.Remove(item);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        private static CartDto MapToDto(Cart cart)
        {
            var items = cart.Items.Select(i => {
                var price = i.ProductVariant.PriceOverride ?? i.ProductVariant.Product.Price;
                return new CartItemDto(
                    i.Id,
                    i.ProductVariantId,
                    i.ProductVariant.Product.Name,
                    i.ProductVariant.Color?.Name,
                    i.ProductVariant.Size?.Name,
                    i.ProductVariant.Product.MainImageUrl,
                    price,
                    i.Quantity,
                    price * i.Quantity
                );
            }).ToList();

            var totalPrice = items.Sum(i => i.Subtotal);

            return new CartDto(cart.Id, totalPrice, items);
        }
    }
}
