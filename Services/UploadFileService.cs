namespace momken_backend.Services
{
    public class UploadFileService: IUploadFileService
    {
        public async Task<string> UploadFile(IFormFile formFile,bool isPublic = false)
        {
            
            string extention = Path.GetExtension(formFile.FileName);
            if (extention == null) {
               
            }
            long fileSize = formFile.Length;

            string fileName = Guid.NewGuid().ToString()+extention;
            var publicPath = "";
            if (isPublic) {
                 publicPath = "Public";
            }
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", publicPath);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string path = Path.Combine(uploadPath, fileName);
              FileStream fs = new(path,FileMode.Create);
            await formFile.CopyToAsync(fs);
            fs.Dispose();
            fs.Close();
            return fileName;

        }
    }

   public interface IUploadFileService
    {
        Task<string> UploadFile(IFormFile formFile, bool isPublic = false);
    }
}
