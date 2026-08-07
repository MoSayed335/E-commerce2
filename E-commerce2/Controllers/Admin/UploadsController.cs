using ECommerce2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce2.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UploadsController : ControllerBase
    {
        private readonly IFileService _fileService;

        public UploadsController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string folder = "")
        {
            var result = await _fileService.UploadFileAsync(file, folder);
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            
            return Ok(new { ImageUrl = result.Value });
        }

        [HttpPost("product-main-image")]
        public async Task<IActionResult> UploadProductMainImage([FromForm] IFormFile file)
        {
            var result = await _fileService.UploadFileAsync(file, "product/mainimg");
            if (!result.IsSuccess) return BadRequest(new { Message = result.Error });
            
            return Ok(new { ImageUrl = result.Value });
        }
    }
}
