using Domain.Exceptions;
using Domain.FirebaseStorage;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FirebaseStorage.Commands.UploadFile
{
    public record UploadFileCommand : IRequest<Uri>
    {
        public IFormFile File { get; init; } = default;
    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Uri>
    {
        private IFirebaseStorageService _firebaseStorageService;

        public UploadFileCommandHandler(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<Uri> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            if(request.File == null)
            {
                throw new MediaException("File is required!");
            }
            return await _firebaseStorageService.UploadFile(request.File.FileName, request.File);
        }
    }
}
