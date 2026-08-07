using ECommerce2.DTOs;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewAdminService _reviewAdminService;

        public ReviewsController(IReviewAdminService reviewAdminService)
        {
            _reviewAdminService = reviewAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] ReviewQueryParameters parameters)
        {
            var result = await _reviewAdminService.GetAdminReviewsAsync(parameters);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReviewStatusRequest request)
        {
            var result = await _reviewAdminService.UpdateStatusAsync(id, request.Status);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpPut("{id}/pin")]
        public async Task<IActionResult> TogglePin(int id)
        {
            var result = await _reviewAdminService.TogglePinAsync(id);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }

        [HttpGet("urgency-counter")]
        public async Task<IActionResult> GetLiveUrgencyCounter()
        {
            var counter = await _reviewAdminService.GetLiveUrgencyCounterAsync();
            return Ok(new { baseCounter = counter });
        }

        [HttpPut("urgency-counter")]
        public async Task<IActionResult> UpdateLiveUrgencyCounter([FromBody] UpdateLiveUrgencyDto request)
        {
            var result = await _reviewAdminService.UpdateLiveUrgencyCounterAsync(request.BaseCounter);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return Ok();
        }
    }
}
