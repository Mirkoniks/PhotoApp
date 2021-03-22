using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using static PhotoApp.Common.DataValidations.User;

namespace PhotoApp.Data.Models
{
    public class PhotoAppUser : IdentityUser
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public AccountUserPhoto ProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CoverPhotoId { get; set; }

        public int ProfilePicId { get; set; }

        public ICollection<UserPhoto> UsersPhotos { get; set; } = new HashSet<UserPhoto>();

        public ICollection<UsersPhotoLikes> UsersPhotoLikes { get; set; } = new HashSet<UsersPhotoLikes>();

        public ICollection<UserNotification> UserNotifications { get; set; } = new HashSet<UserNotification>();

        public ICollection<UserWonChallange> UserWonChallanges { get; set; } = new HashSet<UserWonChallange>();
    }
}
