using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce2.Controllers.Storefront
{
    [ApiController]
    [Route("api/storefront/[controller]")]
    [Authorize]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCoupon([FromBody] string code)
        {
            var userId = GetUserId();
            var result = await _couponService.ValidateCouponAsync(code, userId);
            
            if (!result.IsSuccess)
                return BadRequest(new { Message = result.Error });

            return Ok(new { DiscountAmount = result.Value });
        }
    }
}
