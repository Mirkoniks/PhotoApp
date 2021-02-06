using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Photo
{
    public class UserPhotosServiceModel
    {
        public IEnumerable<UserPhotoServiceModel> Photo { get; set; }
    }
}
