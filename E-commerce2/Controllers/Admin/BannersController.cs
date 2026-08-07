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
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<BannerDto>>> GetBanners([FromQuery] BannerQueryParameters parameters)
        {
            var result = await _bannerService.GetAdminBannersAsync(parameters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerDto dto)
        {
            var result = await _bannerService.CreateAsync(dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return CreatedAtAction(nameof(GetBanners), new { id = result.Value }, new { Id = result.Value, Message = "Banner created." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] UpdateBannerDto dto)
        {
            var result = await _bannerService.UpdateAsync(id, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Banner updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var result = await _bannerService.DeleteAsync(id);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            return Ok(new { Message = "Banner deleted successfully." });
        }
    }
}
