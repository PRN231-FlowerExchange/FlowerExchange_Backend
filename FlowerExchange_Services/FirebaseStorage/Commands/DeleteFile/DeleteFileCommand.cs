using Domain.FirebaseStorage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FirebaseStorage.Commands.DeleteFile
{
    public record DeleteFileCommand : IRequest<Boolean>
    {
        public string FileName { get; init; } = default;
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Boolean>
    {
        private IFirebaseStorageService _firebaseStorageService;

        public DeleteFileCommandHandler(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            bool result = false;
            try
            {
                await _firebaseStorageService.DeleteFile(request.FileName);
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
