using Microsoft.AspNetCore.Identity;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.Models.Photo;
using PhotoApp.Services.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly PhotoAppDbContext dbContext;
        private readonly UserManager<PhotoAppUser> userManager;

        public UserService(PhotoAppDbContext dbContext,
                           UserManager<PhotoAppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task AssignUserToPhotoAsync(string userId, int photoId)
        {
            var model = new UserPhoto
            {
                UserId = userId,
                PhotoId = photoId
            };

            await dbContext.UsersPhotos.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 0 - member
        /// 1 - moderator
        /// 2 - admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns>
        /// 0 - user has no roles
        /// 1 - successful
        /// </returns>
        public async Task<int> GiveRole(string userId, int role)
        {
            var user = await GetUserAsync(userId);

            switch (role)
            {
                case 0:
                    await userManager.AddToRoleAsync(user, "Member");
                    break;
                case 1:
                    await userManager.AddToRoleAsync(user, "Moderator");
                    break;
                case 2:
                    await userManager.AddToRoleAsync(user, "Admin");
                    break;
                default:
                    break;
            }

            return 1;
        }

        /// <summary>
        /// 0 - member
        /// 1 - moderator
        /// 2 - admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns>
        /// 0 - user has no roles
        /// 1 - successful
        /// </returns>
        public async Task<int> RemoveRole(string userId, int role)
        {
            var user = await GetUserAsync(userId);

            switch (role)
            {
                case 0:
                    await userManager.RemoveFromRoleAsync(user, "Member");
                    break;
                case 1:
                    await userManager.RemoveFromRoleAsync(user, "Moderator");
                    break;
                case 2:
                    await userManager.RemoveFromRoleAsync(user, "Admin");
                    break;
            }

            return 1;
        }

        /// <summary>
        /// This method gets a user by group depending what number the role param is
        /// 0 - member
        /// 1 - moderator
        /// 2 - admin
        /// 3 - all groups(default)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<UserServiceModel> GetUserById(string userId)
        {
            var user = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();

            UserServiceModel serviceModel = new UserServiceModel
            {
                UserName = user.UserName,
                Birthday = user.Birthday,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                RegisterDate = user.CreatedOn
            };

            return serviceModel;
        }

        /// <summary>
        /// This method gets users by groups depending what number the role param is
        /// 0 - member
        /// 1 - moderator
        /// 2 - admin
        /// 3 - all groups(default)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<UsersServiceModel> GetAllUsers(int role = 3)
        {

            var userIds = await GetUserIdFromRoles(role);
            UsersServiceModel usersServiceModels = new UsersServiceModel();
            List<UserServiceModel> userServiceModels = new List<UserServiceModel>();

            foreach (var item in userIds)
            {
                userServiceModels.Add(await GetUserById(item.UserId));
            }

            usersServiceModels.Users = userServiceModels;

            return usersServiceModels;
        }

        public async Task<int> GetTotalUsersCount()
        {
            int usersCount = dbContext.Users.Count();

            return usersCount;
        }

        public async Task<int> GetUsersCountFromToday()
        {
            DateTime now = DateTime.UtcNow.Date;

            int usersCount = dbContext.Users.Where(u => u.CreatedOn.Date == now).Count();

            return usersCount;
        }


        private async Task<List<IdentityUserRole<string>>> GetUserIdFromRoles(int role)
        {
            List<IdentityUserRole<string>> users = default;
            string roleId;

            switch (role)
            {
                case 1:
                    roleId = dbContext.Roles.Where(r => r.Name == "Moderator").FirstOrDefault().Id;
                    users = dbContext.UserRoles.Where(ur => ur.RoleId == roleId).ToList();
                    break;
                case 2:
                    roleId = dbContext.Roles.Where(r => r.Name == "Admin").FirstOrDefault().Id;
                    users = dbContext.UserRoles.Where(ur => ur.RoleId == roleId).ToList();
                    break;
            }

            return users;
        }

        private async Task<PhotoAppUser> GetUserAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }
    }
}
