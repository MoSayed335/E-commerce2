using ECommerce2.DTOs.Responses;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerAdminService _customerService;

        public CustomersController(ICustomerAdminService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<ECommerce2.Utilities.PaginatedList<AdminCustomerSummaryDto>>> GetCustomers(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchQuery = null)
        {
            var customers = await _customerService.GetAdminCustomersAsync(pageIndex, pageSize, searchQuery);
            return Ok(customers);
        }
    }
}
