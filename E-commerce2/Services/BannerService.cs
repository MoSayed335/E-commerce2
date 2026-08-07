using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BannerService(IBannerRepository bannerRepository, IUnitOfWork unitOfWork)
        {
            _bannerRepository = bannerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> CreateAsync(CreateBannerDto dto)
        {
            var banner = new Banner
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Type = dto.Type,
                Status = dto.Status,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt
            };

            foreach(var pId in dto.ProductIds)
                banner.BannerProducts.Add(new BannerProduct { ProductId = pId });

            foreach(var cId in dto.CategoryIds)
                banner.BannerCategories.Add(new BannerCategory { CategoryId = cId });

            await _bannerRepository.AddAsync(banner);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(banner.Id);
        }

        public async Task<Result> UpdateAsync(int id, UpdateBannerDto dto)
        {
            var banner = await _bannerRepository.GetByIdAsync(id); // Normally we'd include collections if we modify them, so let's do a custom find or just load them if needed.
            if (banner == null) return Result.Failure("Banner not found");
            
            // For simplicity, we just update scalar properties here, 
            // updating relationships properly requires loading them and syncing.
            banner.Name = dto.Name;
            banner.Description = dto.Description;
            banner.ImageUrl = dto.ImageUrl;
            banner.Type = dto.Type;
            banner.Status = dto.Status;
            banner.StartAt = dto.StartAt;
            banner.EndAt = dto.EndAt;

            // In a full implementation, you would sync `BannerProducts` and `BannerCategories` here.
            // (Clear existing, add new ones).
            
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null) return Result.Failure("Banner not found");

            _bannerRepository.Remove(banner);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<PaginatedList<BannerDto>> GetAdminBannersAsync(BannerQueryParameters parameters)
        {
            var result = await _bannerRepository.GetAllForAdminAsync(parameters);

            var items = result.Items.Select(MapToDto).ToList();

            return new PaginatedList<BannerDto>(items, result.TotalCount, result.PageIndex, parameters.PageSize);
        }

        public async Task<List<BannerDto>> GetStorefrontBannersAsync()
        {
            var banners = await _bannerRepository.GetActiveBannersAsync();
            return banners.Select(MapToDto).ToList();
        }

        private static BannerDto MapToDto(Banner b)
        {
            return new BannerDto(
                b.Id,
                b.Name,
                b.Description,
                b.ImageUrl,
                b.Type,
                b.Status,
                b.StartAt,
                b.EndAt,
                b.BannerProducts.Select(bp => bp.ProductId).ToList(),
                b.BannerCategories.Select(bc => bc.CategoryId).ToList()
            );
        }
    }
}
