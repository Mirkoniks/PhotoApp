using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.User
{
    public class UsersServiceModel
    {
        public IEnumerable<UserServiceModel> Users { get; set; }
    }
}
