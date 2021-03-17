using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class UsersPhotoLikes
    {
        public string UserId { get; set; }
        public PhotoAppUser User { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

        public int ChallangeId { get; set; }
    }
}
