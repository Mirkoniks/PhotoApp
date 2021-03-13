using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Registered on")]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Member")]
        public bool IsMember { get; set; }

        [Display(Name = "Moderator")]
        public bool IsModerator { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }
    }
}
