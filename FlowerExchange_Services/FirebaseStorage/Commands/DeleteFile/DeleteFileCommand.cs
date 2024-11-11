using Domain.Cloudinary;
using MediatR;

namespace Application.FirebaseStorage.Commands.DeleteFile
{
    public record DeleteFileCommand : IRequest<Boolean>
    {
        public string FileName { get; init; } = default;
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Boolean>
    {
        private ICloudinaryService _cloudinaryService;

        public DeleteFileCommandHandler(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            bool result = false;
            try
            {
                var response = await _cloudinaryService.DeleteImageAsync(request.FileName);

                if(response.Result != "ok")
                {
                    return false;
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
