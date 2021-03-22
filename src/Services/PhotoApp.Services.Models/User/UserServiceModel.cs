using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.User
{
    public class UserServiceModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool IsMember { get; set; }

        public bool IsModerator { get; set; }

        public bool IsAdmin { get; set; }

        public string Role { get; set; }

        public int CoverPhotoId { get; set; }

        public int ProfilePicId { get; set; }
    }
}
