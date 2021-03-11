using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.User
{
    public class UserServiceModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
