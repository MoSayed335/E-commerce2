using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponAdminService _couponAdminService;

        public CouponsController(ICouponAdminService couponAdminService)
        {
            _couponAdminService = couponAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoupons([FromQuery] CouponQueryParameters parameters)
        {
            var result = await _couponAdminService.GetAdminCouponsAsync(parameters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponDto request)
        {
            var result = await _couponAdminService.CreateAsync(request);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok(new { id = result.Value });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateCouponStatusRequest request)
        {
            var result = await _couponAdminService.UpdateStatusAsync(id, request.Status);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var result = await _couponAdminService.DeleteAsync(id);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpGet("tracking")]
        public async Task<IActionResult> GetCampaignTracking()
        {
            var trackingStats = await _couponAdminService.GetCampaignTrackingStatsAsync();
            return Ok(trackingStats);
        }
    }

    public record UpdateCouponStatusRequest(bool Status);
}
