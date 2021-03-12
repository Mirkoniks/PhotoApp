using PhotoApp.Services.Models.Photo;
using PhotoApp.Services.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.UserService
{
    public interface IUserService
    {
        public Task AssignUserToPhotoAsync(string userId, int photoId);

        public Task<int> GiveRole(string userId, int role);

        public Task<int> RemoveRole(string userId, int role);

        public Task<UserServiceModel> GetUserById(string userId);

        public Task<UsersServiceModel> GetAllUsers(int role);

        public Task<int> GetTotalUsersCount();

        public Task<int> GetUsersCountFromToday();

    }
}
