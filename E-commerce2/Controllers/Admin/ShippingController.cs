using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _shippingService.GetSettingsAsync();
            return Ok(settings);
        }

        [HttpPut("threshold")]
        public async Task<IActionResult> UpdateThreshold([FromBody] UpdateFreeShippingThresholdRequest request)
        {
            var result = await _shippingService.UpdateFreeShippingThresholdAsync(request.Threshold);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpPut("governorates")]
        public async Task<IActionResult> UpdateGovernorates([FromBody] UpdateShippingRatesRequest request)
        {
            var result = await _shippingService.UpdateGovernoratesAsync(request.Governorates);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpPost("governorates")]
        public async Task<IActionResult> AddGovernorate([FromBody] CreateGovernorateDto request)
        {
            var result = await _shippingService.CreateGovernorateAsync(request);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpDelete("governorates/{id}")]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            var result = await _shippingService.DeleteGovernorateAsync(id);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }
    }
}
