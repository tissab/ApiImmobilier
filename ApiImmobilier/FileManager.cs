using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier
{
    public class FileManager
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public FileManager(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            String filename = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(10).ToArray()).Replace(' ', '-');
            filename = filename + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
            var imagePath = $"{this._hostEnvironment.WebRootPath}\\Files\\{filename}";
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            String imagename = new String(Path.GetFileNameWithoutExtension(image.FileName).Take(10).ToArray()).Replace(' ', '-');
            imagename = imagename + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(image.FileName);
            var imagePath = $"{this._hostEnvironment.WebRootPath}\\Images\\{imagename}";
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return imagename;
        }

        public void DeleteFile(string filegename)
        {
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "Files", filegename);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        public void DeleteImage(string filegename)
        {
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", filegename);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

    }
}
