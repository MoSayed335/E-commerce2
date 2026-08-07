using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class CouponAdminService : ICouponAdminService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CouponAdminService(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> CreateAsync(CreateCouponDto dto)
        {
            var existing = await _couponRepository.GetByCodeAsync(dto.Code);
            if (existing != null)
                return Result<int>.Failure("كود الكوبون موجود مسبقاً.");

            var coupon = new Coupon
            {
                Code = dto.Code.ToUpper(),
                Name = dto.Name,
                DiscountType = dto.DiscountType,
                Value = dto.Value,
                MinOrderAmount = dto.MinOrderAmount,
                MaxDiscountAmount = dto.MaxDiscountAmount,
                UsageLimit = dto.UsageLimit,
                StartDate = dto.StartDate,
                ExpiryDate = dto.ExpiryDate,
                Status = true
            };

            await _couponRepository.AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(coupon.Id);
        }

        public async Task<PaginatedList<AdminCouponSummaryDto>> GetAdminCouponsAsync(CouponQueryParameters parameters)
        {
            var coupons = await _couponRepository.GetAllForAdminAsync(parameters);

            var summaryList = coupons.Items.Select(c => new AdminCouponSummaryDto(
                c.Id,
                c.Code,
                c.Name,
                c.DiscountType,
                c.Value,
                c.MinOrderAmount,
                c.MaxDiscountAmount,
                c.UsageLimit,
                c.TimesUsed,
                c.StartDate,
                c.ExpiryDate,
                c.Status
            )).ToList();

            return new PaginatedList<AdminCouponSummaryDto>(
                summaryList, 
                coupons.TotalCount, 
                coupons.PageIndex, 
                parameters.PageSize);
        }

        public async Task<Result> UpdateStatusAsync(int id, bool status)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
                return Result.Failure("الكوبون غير موجود.");

            coupon.Status = status;
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
                return Result.Failure("الكوبون غير موجود.");

            _couponRepository.Remove(coupon);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<List<CampaignTrackingDto>> GetCampaignTrackingStatsAsync()
        {
            return await _couponRepository.GetCampaignTrackingStatsAsync();
        }
    }
}
