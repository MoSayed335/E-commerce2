using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Models.Enums;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IReviewRepository reviewRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<StorefrontReviewDto>> GetProductReviewsAsync(int productId, int pageIndex, int pageSize)
        {
            var pagedReviews = await _reviewRepository.GetApprovedReviewsForProductAsync(productId, pageIndex, pageSize);

            var items = pagedReviews.Items.Select(r => new StorefrontReviewDto(
                r.Id,
                $"{r.User.FName} {r.User.LName}",
                r.Rating,
                r.Comment,
                r.ImageUrl,
                r.IsPinned,
                r.CreatedAt
            )).ToList();

            return new PaginatedList<StorefrontReviewDto>(items, pagedReviews.TotalCount, pagedReviews.PageIndex, pageSize);
        }

        public async Task<Result> AddReviewAsync(string userId, CreateReviewDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                return Result.Failure("Rating must be between 1 and 5.");

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                return Result.Failure("Product not found.");

            var review = new Review
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ImageUrl = dto.ImageUrl,
                HasPhoto = !string.IsNullOrEmpty(dto.ImageUrl),
                Status = ReviewStatus.Pending,
                IsPinned = false
            };

            await _reviewRepository.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
