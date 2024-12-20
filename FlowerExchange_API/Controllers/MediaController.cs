﻿using Application.FirebaseStorage.Commands.DeleteFile;
using Application.FirebaseStorage.Commands.UploadFile;
using Domain.FirebaseStorage;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : APIControllerBase
    {
        private readonly ILogger<MediaController> _logger;

        public MediaController(ILogger<MediaController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(UploadFileCommand command)
        {
            var fileUploaded =  await Mediator.Send(command);
            return Ok(fileUploaded);
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
