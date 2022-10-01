using Bookshop.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Service
{
    public class FormFileService : IFormFileService
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FormFileService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            this.environment = environment;
            this._httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadFileAsync(IFormFile fileService, string folderName)
        {

            string path1 = environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(path1))
            {
                path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            string internalFolderName = Path.Combine(path1, "Images", folderName);

            if (!Directory.Exists(internalFolderName))
            {
                Directory.CreateDirectory(internalFolderName);
            }

            string savingPath = Path.Combine(internalFolderName, fileService.FileName);
         
            using var fileStream = new FileStream(savingPath, FileMode.Create);
            await fileService.CopyToAsync(fileStream);

            var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var pathForDatabase = Path.Combine(currentUrl, "Images", folderName, fileService.FileName).Replace("\\", "/");
            //string pathForDatabase = $"{currentUrl}/{path1}/Images/{folderName}/{fileService.FileName}";
            return pathForDatabase;
        }
    }
}
