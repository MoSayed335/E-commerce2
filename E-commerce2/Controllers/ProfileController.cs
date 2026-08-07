using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var userId = GetUserId();
            var result = await _profileService.GetProfileAsync(userId);
            if (!result.IsSuccess) return NotFound(new { Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPost("addresses")]
        public async Task<IActionResult> AddAddress([FromBody] CreateUserAddressDto dto)
        {
            var userId = GetUserId();
            var result = await _profileService.AddAddressAsync(userId, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            
            return Ok(new { Message = "Address added.", Id = result.Value });
        }

        [HttpPut("addresses/{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateUserAddressDto dto)
        {
            var userId = GetUserId();
            var result = await _profileService.UpdateAddressAsync(userId, id, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });

            return Ok(new { Message = "Address updated." });
        }

        [HttpDelete("addresses/{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = GetUserId();
            var result = await _profileService.DeleteAddressAsync(userId, id);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });

            return Ok(new { Message = "Address deleted." });
        }
    }
}
