using Microsoft.AspNetCore.Http;

namespace Domain.FirebaseStorage
{
    public interface IFirebaseStorageService
    {
        Task<Uri> UploadFile(string name, IFormFile file);
        Task DeleteFile(string fileName);
    }
}
