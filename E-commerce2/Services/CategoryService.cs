using ECommerce2.DTOs;
using ECommerce2.Models;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;

namespace ECommerce2.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId,
                Status = dto.Status
            };

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(category.Id);
        }

        public async Task<Result> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return Result.Failure("Category not found");

            if (dto.ParentCategoryId.HasValue && dto.ParentCategoryId.Value == id)
            {
                return Result.Failure("Category cannot be its own parent.");
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentCategoryId = dto.ParentCategoryId;
            category.Status = dto.Status;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return Result.Failure("Category not found");

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<PaginatedList<CategoryDto>> GetAdminCategoriesAsync(CategoryQueryParameters parameters)
        {
            var result = await _categoryRepository.GetAllForAdminAsync(parameters);

            var items = result.Items.Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Description,
                c.Status,
                c.ParentCategoryId,
                null
            )).ToList();

            return new PaginatedList<CategoryDto>(items, result.TotalCount, result.PageIndex, parameters.PageSize);
        }

        public async Task<List<CategoryDto>> GetStorefrontCategoriesAsync()
        {
            var categories = await _categoryRepository.GetActiveCategoriesWithSubcategoriesAsync();
            return categories.Select(MapToDto).ToList();
        }

        private static CategoryDto MapToDto(Category c)
        {
            return new CategoryDto(
                c.Id,
                c.Name,
                c.Description,
                c.Status,
                c.ParentCategoryId,
                c.SubCategories != null && c.SubCategories.Any() 
                    ? c.SubCategories.Where(sc => sc.Status).Select(MapToDto).ToList() 
                    : new List<CategoryDto>()
            );
        }
    }
}
