using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.infrastructure.Helpers
{
    public  static class UploadFileHelper
    {
        public async static Task<string> UploadFile(IFormFile file,string wwwroot,string folderName)
        {
            var extention = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + extention; // unique name
            
            var ImagePath = Path.Combine(wwwroot, "Images/" + folderName);

            if (!Directory.Exists(ImagePath))
                Directory.CreateDirectory(ImagePath);

            var filePath = Path.Combine(ImagePath, fileName);

            try
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                return "Error uploading file : " + ex;
            }

            return "Images/" + folderName + "/" + fileName;
        }
    }
}
