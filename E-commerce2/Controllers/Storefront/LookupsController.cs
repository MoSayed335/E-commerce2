using ECommerce2.DTOs;
using ECommerce2.Repositories.Interfaces;
using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Storefront
{
    [ApiController]
    [Route("api/storefront/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        private readonly IGovernorateRepository _governorateRepository;

        public LookupsController(IAttributeService attributeService, IGovernorateRepository governorateRepository)
        {
            _attributeService = attributeService;
            _governorateRepository = governorateRepository;
        }

        [HttpGet("governorates")]
        public async Task<IActionResult> GetGovernorates()
        {
            var governorates = await _governorateRepository.GetAllAsync();
            var dtos = governorates.Select(g => new
            {
                g.Id,
                g.NameEn,
                g.NameAr,
                g.Fee,
                g.EstimatedDelivery
            }).ToList();
            
            return Ok(dtos);
        }

        [HttpGet("colors")]
        public async Task<ActionResult<List<ColorDto>>> GetColors()
        {
            return Ok(await _attributeService.GetColorsAsync());
        }

        [HttpGet("sizes")]
        public async Task<ActionResult<List<SizeDto>>> GetSizes()
        {
            return Ok(await _attributeService.GetSizesAsync());
        }
    }
}
