using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce2.Controllers.Storefront
{
    [ApiController]
    [Route("api/storefront/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public CheckoutController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            var userId = GetUserId();
            var result = await _orderService.CheckoutAsync(userId, dto);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });

            return Ok(result.Value);
        }
    }
}
