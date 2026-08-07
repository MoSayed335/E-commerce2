using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using ECommerce2.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<CategoryDto>>> GetCategories([FromQuery] CategoryQueryParameters parameters)
        {
            var result = await _categoryService.GetAdminCategoriesAsync(parameters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateAsync(dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return CreatedAtAction(nameof(GetCategories), new { id = result.Value }, new { Id = result.Value, Message = "Category created." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateAsync(id, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Category updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Category deleted successfully." });
        }
    }
}
