using ECommerce2.DTOs.Responses;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteService(IFavoriteRepository favoriteRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId)
        {
            var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);
            
            return favorites.Select(f => new FavoriteDto
            {
                Id = f.Id,
                ProductId = f.ProductId,
                ProductName = f.Product.Name,
                ProductSlug = null,
                Price = f.Product.Price,
                OldPrice = null,
                ImageUrl = f.Product.MainImageUrl
            }).ToList();
        }

        public async Task<Result> AddToFavoritesAsync(string userId, int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result.Failure("Product not found.");
            }

            var existing = await _favoriteRepository.GetUserFavoriteAsync(userId, productId);
            if (existing != null)
            {
                return Result.Success(); // Already favorited
            }

            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = productId
            };

            await _favoriteRepository.AddAsync(favorite);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> RemoveFromFavoritesAsync(string userId, int productId)
        {
            var favorite = await _favoriteRepository.GetUserFavoriteAsync(userId, productId);
            if (favorite == null)
            {
                return Result.Failure("Favorite not found.");
            }

            _favoriteRepository.Remove(favorite);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
