using Domain.FirebaseStorage;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FirebaseStorage
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private const string BucketName = "flower-exchange-media.appspot.com";

        public FirebaseStorageService(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task DeleteFile(string fileName)
        {
            await _storageClient.DeleteObjectAsync(BucketName, fileName);
        }

        public async Task<Uri> UploadFile(string name, IFormFile file)
        {
            var randomGuid = Guid.NewGuid();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var blob = await _storageClient.UploadObjectAsync(BucketName,
                $"{name}-{randomGuid}", file.ContentType, stream);
            var photoUri = new Uri(blob.MediaLink);
            return photoUri;
        }
    }
}
