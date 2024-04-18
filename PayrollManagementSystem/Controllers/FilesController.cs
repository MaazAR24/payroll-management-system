using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Services.FileUploader;

namespace PayrollManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = await _fileService.Get(id);

            if (file == null)
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, file.ContentType, file.FileName);
        }

        [Authorize]
        [HttpGet("{moduleId}/{recordId}")]
        public async Task<IActionResult> GetFiles(int moduleId, int recordId)
        {
            var files = await _fileService.Get(moduleId, recordId);
            return Ok(files);


        }

    }
}
