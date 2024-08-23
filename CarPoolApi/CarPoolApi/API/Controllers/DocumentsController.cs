using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public DocumentsController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var fileName = Path.GetFileName(file.FileName);
            using (var stream = file.OpenReadStream())
            {
                var url = await _blobService.UploadDocumentAsync(stream, fileName);
                return Ok(new { Url = url });
            }
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadDocument(string fileName)
        {
            var stream = await _blobService.DownloadDocumentAsync(fileName);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", fileName);
        }
    }

}

