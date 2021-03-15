using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PhotoApp.Services.CloudinaryService
{
    public class CloudinaryService : ICloundinaryService
    {
        public async Task<List<string>> UploadAsync(Cloudinary cloudinary, ICollection<IFormFile> files)
        {
            List<Task<string>> tasks = new List<Task<string>>();
            List<string> imageUrls = new List<string>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => Upload(cloudinary, file)));
            }

            foreach (var item in tasks)
            {
                imageUrls.Add(await item);
            }

            return imageUrls;
        }

        private async Task<string> Upload(Cloudinary cloudinary, IFormFile file)
        {
            byte[] destiantionImage;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destiantionImage = memoryStream.ToArray();
            }

            using (var destinationStream = new MemoryStream(destiantionImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStream)
                };
                var response = await cloudinary.UploadAsync(uploadParams);

                return (response.SecureUrl.AbsoluteUri);
            }
        }
    }
}
