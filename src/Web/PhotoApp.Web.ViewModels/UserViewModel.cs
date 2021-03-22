using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Web.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string ProfilePicLink { get; set; }

        public string CoverPhotoLink { get; set; }

        public int PhotosCount { get; set; }

        public List<PhotoViewModel> PhotoLinks { get; set; }
    }
}
