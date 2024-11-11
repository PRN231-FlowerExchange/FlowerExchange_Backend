using Domain.Cloudinary;
using Domain.Exceptions;
using Domain.FirebaseStorage.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.FirebaseStorage.Commands.UploadFile
{
    public record UploadFileCommand : IRequest<FileUploadedResponse>
    {
        public IFormFile File { get; init; } = default;
    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileUploadedResponse>
    {
        private ICloudinaryService _cloudinaryService;

        public UploadFileCommandHandler(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<FileUploadedResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.File == null)
                {
                    throw new MediaException("File is required!");
                }

                var result = await _cloudinaryService.UploadImageAsync(request.File);

                if (result.Error != null)
                {
                    throw new Exception(result.Error.Message);
                }

                return new FileUploadedResponse
                {
                    Uri = result.SecureUrl,
                    FileName = result.PublicId
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
