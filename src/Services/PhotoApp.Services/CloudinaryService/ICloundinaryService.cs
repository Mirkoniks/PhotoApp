using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.CloudinaryService
{
    public interface ICloundinaryService
    {
        public Task<List<string>> UploadAsync(Cloudinary cloudinary, ICollection<IFormFile> files);
    }
}
