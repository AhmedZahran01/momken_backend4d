using Microsoft.AspNetCore.Mvc;
using System;

namespace momken_backend.Services
{
    public class TempImgService: ITempImgService
    {
        public string Upload(string imgPath)
        {

            // Generate unique file name
            var fileName = Guid.NewGuid()+"_"+ imgPath;
            var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Public", "temp_images");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            var tempImagePath = Path.Combine(Directory.GetCurrentDirectory(), tempPath, fileName.ToString());
            var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads",imgPath);

            // Save the image in wwwroot/temp_images
             File.Copy(sourcePath, tempImagePath, overwrite: true);
            // Return public URL of the image
            var imageUrl = $"public/temp_images/{fileName}";

            // Schedule file deletion after 10 minutes
            _ = Task.Run(() => DeleteFileAfterDelay(tempImagePath, TimeSpan.FromMinutes(10)));
            return $"{imageUrl}";
        }
            private async Task DeleteFileAfterDelay(string filePath, TimeSpan delay)
        {
            await Task.Delay(delay);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
    public interface ITempImgService
    {
        string Upload(string imgPath);
    }
}
