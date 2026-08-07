using ECommerce2.Utilities;
using ECommerce2.DTOs;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using ECommerce2.Models;

namespace ECommerce2.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetWithVariantsAsync(id);
            if (product is null) return null;

            return new ProductDetailsDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.DiscountPercentage,
                product.Category?.Name ?? string.Empty,
                product.Images.Select(i => i.ImageUrl).ToList(),
                product.Variants.Select(v => new ProductVariantDto(
                    v.Id,
                    v.Color?.Name,
                    v.Color?.HexCode,
                    v.Size?.Name,
                    v.Stock,
                    v.PriceOverride ?? product.Price
                )).ToList()
            );
        }

        public async Task<IReadOnlyList<ProductListItemDto>> GetByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products
                .Select(p => new ProductListItemDto(p.Id, p.Name, p.Price, p.MainImageUrl, p.Status))
                .ToList();
        }

        public async Task<Result<int>> CreateAsync(CreateProductDto dto)
        {
            if (dto.Price <= 0)
                return Result<int>.Failure("السعر يجب أن يكون أكبر من صفر.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                MainImageUrl = dto.MainImageUrl,
                Sku = $"PRD-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            };

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(product.Id);
        }

        public async Task<PaginatedList<AdminProductSummaryDto>> GetAdminProductsAsync(ProductQueryParameters parameters)
        {
            var pagedProducts = await _productRepository.GetAllForAdminAsync(parameters);

            var items = pagedProducts.Items.Select(p => new AdminProductSummaryDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category?.Name ?? string.Empty,
                p.Variants.Sum(v => v.Stock),
                p.Status,
                p.MainImageUrl,
                p.CreatedAt
            )).ToList();

            return new PaginatedList<AdminProductSummaryDto>(
                items,
                pagedProducts.TotalCount,
                pagedProducts.PageIndex,
                parameters.PageSize
            );
        }

        public async Task<Result> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return Result.Failure("المنتج غير موجود.");

            if (dto.Price <= 0)
                return Result.Failure("السعر يجب أن يكون أكبر من صفر.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.MainImageUrl = dto.MainImageUrl;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return Result.Failure("المنتج غير موجود.");

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateStatusAsync(int id, bool status)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return Result.Failure("المنتج غير موجود.");

            product.Status = status;
            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
