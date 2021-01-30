using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }

        public string Link { get; set; }

        public ICollection<UserPhoto> UsersPhotos { get; set; } = new HashSet<UserPhoto>();

        public ICollection<PhotoChallange> PhotosChallanges { get; set; } = new HashSet<PhotoChallange>();

        public ICollection<UsersPhotoLikes> UsersPhotoLikes { get; set; } = new HashSet<UsersPhotoLikes>();
    }
}
