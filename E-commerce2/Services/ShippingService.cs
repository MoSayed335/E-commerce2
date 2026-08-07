using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IGovernorateRepository _governorateRepository;
        private readonly IStoreSettingRepository _storeSettingRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        private const string FreeShippingThresholdKey = "FreeShippingThreshold";

        public ShippingService(
            IGovernorateRepository governorateRepository,
            IStoreSettingRepository storeSettingRepository,
            IUnitOfWork unitOfWork)
        {
            _governorateRepository = governorateRepository;
            _storeSettingRepository = storeSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ShippingSettingsDto> GetSettingsAsync()
        {
            var thresholdSetting = await _storeSettingRepository.GetByKeyAsync(FreeShippingThresholdKey);
            decimal threshold = 0;
            if (thresholdSetting != null && decimal.TryParse(thresholdSetting.Value, out var parsed))
            {
                threshold = parsed;
            }

            var governorates = await _governorateRepository.GetAllAsync();
            var governorateDtos = governorates.Select(g => new GovernorateDto(
                g.Id,
                g.NameEn,
                g.NameAr,
                g.Fee,
                g.EstimatedDelivery
            )).ToList();

            return new ShippingSettingsDto(threshold, governorateDtos);
        }

        public async Task<Result> UpdateFreeShippingThresholdAsync(decimal threshold)
        {
            var setting = await _storeSettingRepository.GetByKeyAsync(FreeShippingThresholdKey);
            if (setting == null)
            {
                setting = new StoreSetting
                {
                    Key = FreeShippingThresholdKey,
                    Value = threshold.ToString()
                };
                await _storeSettingRepository.AddAsync(setting);
            }
            else
            {
                setting.Value = threshold.ToString();
                _storeSettingRepository.Update(setting);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateGovernoratesAsync(List<UpdateGovernorateDto> governorates)
        {
            foreach (var update in governorates)
            {
                var gov = await _governorateRepository.GetByIdAsync(update.Id);
                if (gov != null)
                {
                    gov.Fee = update.Fee;
                    gov.EstimatedDelivery = update.EstimatedDelivery;
                    _governorateRepository.Update(gov);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> CreateGovernorateAsync(CreateGovernorateDto dto)
        {
            var gov = new Governorate
            {
                NameEn = dto.NameEn,
                NameAr = dto.NameAr,
                Fee = dto.Fee,
                EstimatedDelivery = dto.EstimatedDelivery
            };

            await _governorateRepository.AddAsync(gov);
            await _unitOfWork.SaveChangesAsync();
            
            return Result.Success();
        }

        public async Task<Result> DeleteGovernorateAsync(int id)
        {
            var gov = await _governorateRepository.GetByIdAsync(id);
            if (gov == null)
                return Result.Failure("المحافظة غير موجودة.");

            _governorateRepository.Remove(gov);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
