using Domain.Exceptions;
using Domain.FirebaseStorage;
using Domain.FirebaseStorage.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FirebaseStorage.Commands.UploadFile
{
    public record UploadFileCommand : IRequest<FileUploadedResponse>
    {
        public IFormFile File { get; init; } = default;
    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileUploadedResponse>
    {
        private IFirebaseStorageService _firebaseStorageService;

        public UploadFileCommandHandler(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<FileUploadedResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            if(request.File == null)
            {
                throw new MediaException("File is required!");
            }
            return await _firebaseStorageService.UploadFile(request.File.FileName, request.File);
        }
    }
}
