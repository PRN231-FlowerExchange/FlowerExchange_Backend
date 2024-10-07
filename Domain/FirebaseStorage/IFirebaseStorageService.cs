using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FirebaseStorage
{
    public interface IFirebaseStorageService
    {
        Task<Uri> UploadFile(string name, IFormFile file);
        Task DeleteFile(string fileName);
    }
}
