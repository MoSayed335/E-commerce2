using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SizesController : ControllerBase
    {
        private readonly IAttributeService _attributeService;

        public SizesController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SizeDto>>> GetSizes()
        {
            return Ok(await _attributeService.GetSizesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize([FromBody] CreateSizeDto dto)
        {
            var result = await _attributeService.CreateSizeAsync(dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return CreatedAtAction(nameof(GetSizes), new { id = result.Value }, new { Id = result.Value, Message = "Size created." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSize(int id, [FromBody] UpdateSizeDto dto)
        {
            var result = await _attributeService.UpdateSizeAsync(id, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Size updated." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var result = await _attributeService.DeleteSizeAsync(id);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Size deleted." });
        }
    }
}
