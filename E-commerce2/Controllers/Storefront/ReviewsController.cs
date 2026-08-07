using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce2.Controllers.Storefront
{
    [ApiController]
    [Route("api/storefront/products/{productId}/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<ECommerce2.Utilities.PaginatedList<StorefrontReviewDto>>> GetProductReviews(
            int productId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId, pageIndex, pageSize);
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(int productId, [FromBody] CreateReviewDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (productId != dto.ProductId)
            {
                return BadRequest(new { Message = "Product ID mismatch." });
            }

            var result = await _reviewService.AddReviewAsync(userId, dto);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok(new { Message = "Review submitted successfully and is pending approval." });
        }
    }
}
