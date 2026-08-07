using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AttributeService(IColorRepository colorRepository, ISizeRepository sizeRepository, IUnitOfWork unitOfWork)
        {
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
        }

        // Colors
        public async Task<List<ColorDto>> GetColorsAsync()
        {
            var colors = await _colorRepository.GetAllColorsAsync();
            return colors.Select(c => new ColorDto(c.Id, c.Name, c.HexCode)).ToList();
        }

        public async Task<Result<int>> CreateColorAsync(CreateColorDto dto)
        {
            var color = new Color { Name = dto.Name, HexCode = dto.HexCode };
            await _colorRepository.AddAsync(color);
            await _unitOfWork.SaveChangesAsync();
            return Result<int>.Success(color.Id);
        }

        public async Task<Result> UpdateColorAsync(int id, UpdateColorDto dto)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) return Result.Failure("Color not found.");

            color.Name = dto.Name;
            color.HexCode = dto.HexCode;
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteColorAsync(int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null) return Result.Failure("Color not found.");

            _colorRepository.Remove(color);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        // Sizes
        public async Task<List<SizeDto>> GetSizesAsync()
        {
            var sizes = await _sizeRepository.GetAllSizesAsync();
            return sizes.Select(s => new SizeDto(s.Id, s.Name, s.SortOrder)).ToList();
        }

        public async Task<Result<int>> CreateSizeAsync(CreateSizeDto dto)
        {
            var size = new Size { Name = dto.Name, SortOrder = dto.SortOrder };
            await _sizeRepository.AddAsync(size);
            await _unitOfWork.SaveChangesAsync();
            return Result<int>.Success(size.Id);
        }

        public async Task<Result> UpdateSizeAsync(int id, UpdateSizeDto dto)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null) return Result.Failure("Size not found.");

            size.Name = dto.Name;
            size.SortOrder = dto.SortOrder;
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteSizeAsync(int id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null) return Result.Failure("Size not found.");

            _sizeRepository.Remove(size);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
