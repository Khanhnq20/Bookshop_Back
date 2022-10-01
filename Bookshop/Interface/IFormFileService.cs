using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Interface
{
    public interface IFormFileService
    {
        Task<string> UploadFileAsync(IFormFile fileService, string folderName);
        Task DeleteFileAsync(string filePath);
        Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType);
    }
}
