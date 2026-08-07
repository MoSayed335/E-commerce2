using ECommerce2.DTOs.Responses;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce2.Controllers.Storefront
{
    [ApiController]
    [Route("api/storefront/[controller]")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<List<FavoriteDto>>> GetFavorites()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _favoriteService.AddToFavoritesAsync(userId, productId);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _favoriteService.RemoveFromFavoritesAsync(userId, productId);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok();
        }
    }
}
