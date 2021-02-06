using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class UserLikedPhotosServiceModel
    {
        public IEnumerable<UserLikedPhotoServiceModel> Photos { get; set; }
    }
}
