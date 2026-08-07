using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce2.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(UserManager<User> userManager, IUserAddressRepository addressRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserProfileDto>> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Result<UserProfileDto>.Failure("User not found.");

            var addresses = await _addressRepository.GetUserAddressesAsync(userId);
            var addressDtos = addresses.Select(a => new UserAddressDto(
                a.Id,
                a.FullAddress,
                a.Landmark,
                a.GovernorateId,
                a.Governorate?.NameAr ?? "",
                a.IsDefault
            )).ToList();

            var dto = new UserProfileDto(
                user.Id,
                user.FName,
                user.LName,
                user.Email!,
                user.PhoneNumber ?? "",
                user.CreatedAt,
                addressDtos
            );

            return Result<UserProfileDto>.Success(dto);
        }

        public async Task<Result<int>> AddAddressAsync(string userId, CreateUserAddressDto dto)
        {
            if (dto.IsDefault)
            {
                var existing = await _addressRepository.GetUserAddressesAsync(userId);
                foreach(var addr in existing.Where(a => a.IsDefault))
                {
                    addr.IsDefault = false;
                    _addressRepository.Update(addr);
                }
            }

            var address = new UserAddress
            {
                UserId = userId,
                FullAddress = dto.FullAddress,
                Landmark = dto.Landmark,
                GovernorateId = dto.GovernorateId,
                IsDefault = dto.IsDefault
            };

            await _addressRepository.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(address.Id);
        }

        public async Task<Result> UpdateAddressAsync(string userId, int addressId, UpdateUserAddressDto dto)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId)
                return Result.Failure("Address not found.");

            if (dto.IsDefault && !address.IsDefault)
            {
                var existing = await _addressRepository.GetUserAddressesAsync(userId);
                foreach (var addr in existing.Where(a => a.IsDefault && a.Id != addressId))
                {
                    addr.IsDefault = false;
                    _addressRepository.Update(addr);
                }
            }

            address.FullAddress = dto.FullAddress;
            address.Landmark = dto.Landmark;
            address.GovernorateId = dto.GovernorateId;
            address.IsDefault = dto.IsDefault;

            _addressRepository.Update(address);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAddressAsync(string userId, int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId)
                return Result.Failure("Address not found.");

            _addressRepository.Remove(address);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
