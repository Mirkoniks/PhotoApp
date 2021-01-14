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
            List<string> imageUrls = new List<string>();

            foreach (var file in files)
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

                    imageUrls.Add(response.SecureUrl.AbsoluteUri);
                }
            }

            return imageUrls;
        }
    }
}
