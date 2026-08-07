using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Models.Enums;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class ReviewAdminService : IReviewAdminService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IStoreSettingRepository _storeSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const string LiveUrgencySettingKey = "LiveUrgencyBaseCounter";

        public ReviewAdminService(
            IReviewRepository reviewRepository, 
            IStoreSettingRepository storeSettingRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _storeSettingRepository = storeSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<AdminReviewSummaryDto>> GetAdminReviewsAsync(ReviewQueryParameters parameters)
        {
            var reviews = await _reviewRepository.GetAllForAdminAsync(parameters);

            var summaryList = reviews.Items.Select(r => new AdminReviewSummaryDto(
                r.Id,
                $"{r.User?.FName} {r.User?.LName}".Trim(),
                r.Product?.Name ?? string.Empty,
                r.Rating,
                r.Comment,
                r.HasPhoto,
                r.ImageUrl,
                r.IsPinned,
                r.Status.ToString(),
                r.CreatedAt
            )).ToList();

            return new PaginatedList<AdminReviewSummaryDto>(
                summaryList, 
                reviews.TotalCount, 
                reviews.PageIndex, 
                parameters.PageSize);
        }

        public async Task<Result> UpdateStatusAsync(int id, ReviewStatus status)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return Result.Failure("التقييم غير موجود.");

            review.Status = status;
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> TogglePinAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return Result.Failure("التقييم غير موجود.");

            review.IsPinned = !review.IsPinned;
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<int> GetLiveUrgencyCounterAsync()
        {
            var setting = await _storeSettingRepository.GetByKeyAsync(LiveUrgencySettingKey);
            if (setting != null && int.TryParse(setting.Value, out var counter))
            {
                return counter;
            }
            return 0; // Default if not set
        }

        public async Task<Result> UpdateLiveUrgencyCounterAsync(int baseCounter)
        {
            var setting = await _storeSettingRepository.GetByKeyAsync(LiveUrgencySettingKey);
            if (setting == null)
            {
                setting = new StoreSetting
                {
                    Key = LiveUrgencySettingKey,
                    Value = baseCounter.ToString()
                };
                await _storeSettingRepository.AddAsync(setting);
            }
            else
            {
                setting.Value = baseCounter.ToString();
                _storeSettingRepository.Update(setting);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
