using ECommerce2.Services.Interfaces;
using ECommerce2.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] ECommerce2.DTOs.OrderQueryParameters parameters)
        {
            var orders = await _orderService.GetAdminOrdersAsync(parameters);
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus newStatus)
        {
            var result = await _orderService.UpdateStatusAsync(id, newStatus);
            if (!result.IsSuccess)
                return BadRequest(result.Error);
                
            return Ok();
        }

        [HttpPatch("{id:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }

        public record UpdateNotesRequest(string? Notes);

        [HttpPatch("{id:int}/notes")]
        public async Task<IActionResult> UpdateOrderNotes(int id, [FromBody] UpdateNotesRequest request)
        {
            var result = await _orderService.UpdateAdminNotesAsync(id, request.Notes);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}
