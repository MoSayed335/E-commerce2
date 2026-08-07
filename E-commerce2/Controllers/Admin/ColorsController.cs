using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ColorsController : ControllerBase
    {
        private readonly IAttributeService _attributeService;

        public ColorsController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ColorDto>>> GetColors()
        {
            return Ok(await _attributeService.GetColorsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateColor([FromBody] CreateColorDto dto)
        {
            var result = await _attributeService.CreateColorAsync(dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return CreatedAtAction(nameof(GetColors), new { id = result.Value }, new { Id = result.Value, Message = "Color created." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColor(int id, [FromBody] UpdateColorDto dto)
        {
            var result = await _attributeService.UpdateColorAsync(id, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Color updated." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var result = await _attributeService.DeleteColorAsync(id);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Color deleted." });
        }
    }
}
