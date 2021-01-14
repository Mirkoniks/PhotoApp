using PhotoApp.Services.Models.Photo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.UserService
{
    public interface IUserService
    {
        public Task AssignUserToPhotoAsync(string userId, int photoId);
    }
}
