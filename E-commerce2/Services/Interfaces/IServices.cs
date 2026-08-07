using ECommerce2.Utilities;
using ECommerce2.DTOs;

namespace ECommerce2.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<ProductListItemDto>> GetByCategoryAsync(int categoryId);
        Task<Result<int>> CreateAsync(CreateProductDto dto);
    }

    public interface IOrderService
    {
        Task<Result<OrderDetailsDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDetailsDto?> GetByIdAsync(int orderId);
        Task<IReadOnlyList<OrderDetailsDto>> GetByUserIdAsync(string userId);
        Task<Result> UpdateStatusAsync(int orderId, Models.Enums.OrderStatus newStatus);
    }

    public interface ICartService
    {
        Task<Result> AddItemAsync(string userId, int productVariantId, int quantity);
        Task<Result> RemoveItemAsync(string userId, int cartItemId);
    }
}
