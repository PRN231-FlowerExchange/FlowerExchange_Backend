using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FirebaseStorage.Models
{
    public class FileUploadedResponse
    {
        public Uri Uri { get; set; }

        public string FileName { get; set; }
    }
}
