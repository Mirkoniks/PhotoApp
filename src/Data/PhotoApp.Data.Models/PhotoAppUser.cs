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
        [MaxLength(NameMaxLength)]
        public string Nickname { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string ProfilePictureId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
