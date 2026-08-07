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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddToCartDto dto)
        {
            var userId = GetUserId();
            var result = await _cartService.AddItemAsync(userId, dto);
            if (!result.IsSuccess)
                return BadRequest(new { Message = result.Error });

            return Ok(new { Message = "Item added to cart." });
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateCartItemDto dto)
        {
            var userId = GetUserId();
            var result = await _cartService.UpdateItemQuantityAsync(userId, id, dto.Quantity);
            if (!result.IsSuccess)
                return BadRequest(new { Message = result.Error });

            return Ok(new { Message = "Cart item updated." });
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveItemAsync(userId, id);
            if (!result.IsSuccess)
                return BadRequest(new { Message = result.Error });

            return Ok(new { Message = "Cart item removed." });
        }
    }
}
