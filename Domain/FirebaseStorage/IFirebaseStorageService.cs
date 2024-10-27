using Microsoft.AspNetCore.Http;
using Domain.FirebaseStorage.Models;
using Google.Apis.Storage.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FirebaseStorage
{
    public interface IFirebaseStorageService
    {
        Task<FileUploadedResponse> UploadFile(string name, IFormFile file);
        Task DeleteFile(string fileName);
    }
}
