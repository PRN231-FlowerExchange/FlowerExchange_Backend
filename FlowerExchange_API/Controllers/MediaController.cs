using Application.FirebaseStorage.Commands.DeleteFile;
using Application.FirebaseStorage.Commands.UploadFile;
using Domain.FirebaseStorage;
using Google.Apis.Storage.v1;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaController : APIControllerBase
    {
        private readonly ILogger<MediaController> _logger;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public MediaController(ILogger<MediaController> logger, IFirebaseStorageService firebaseStorageService)
        {
            _logger = logger;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(UploadFileCommand command)
        {
            Uri imageUrl =  await Mediator.Send(command);
            return Ok(imageUrl);
        }

        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            try
            {
                DeleteFileCommand command = new DeleteFileCommand
                {
                    FileName = fileName
                };
                bool result = await Mediator.Send(command);
                if (result)
                {
                    return Ok(new { Message = "File deleted successfully." });
                }
                return Ok(new { Message = "File deleted unsuccessfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting file: {ex.Message}");
            }
        }
    }
}
