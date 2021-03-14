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
                    await userManager.AddToRoleAsync(user, "User");
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
                    await userManager.RemoveFromRoleAsync(user, "User");
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
                Id = user.Id,
                UserName = user.UserName,
                Birthday = user.Birthday,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                RegisterDate = user.CreatedOn
            };

            var roles = await GetUserRole(userId);

            foreach (var item in roles)
            {
                switch (item)
                {
                    case "User":
                        serviceModel.IsMember = true;
                        break;
                    case "Moderator":
                        serviceModel.IsModerator = true;
                        break;
                    case "Admin":
                        serviceModel.IsAdmin = true;
                        break;
                }
            }

            return serviceModel;
        }

        public async Task EditUser(UserServiceModel model)
        {
            var user = dbContext.Users.Where(u => u.Id == model.Id).FirstOrDefault();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.PhoneNumber;
            user.Birthday = model.Birthday;

            if (model.IsMember)
            {
                await GiveRole(model.Id, 0);
            }
            else
            {
                await RemoveRole(model.Id, 0);
            }

            if (model.IsModerator)
            {
                await GiveRole(model.Id, 1);
            }
            else
            {
                await RemoveRole(model.Id, 1);
            }

            if (model.IsAdmin)
            {
                await GiveRole(model.Id, 2);
            }
            else
            {
                await RemoveRole(model.Id, 2);
            }

            await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// 4 - no role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<string>> GetUserRole(string id)
        {
            var roles = dbContext.UserRoles.Where(ur => ur.UserId == id).ToList();

            if (roles != null)
            {
                List<string> roleNames = new List<string>();

                foreach (var item in roles)
                {
                    var rolesDb = dbContext.Roles.Where(r => r.Id == item.RoleId).FirstOrDefault().Name;

                    roleNames.Add(rolesDb);
                }

                return roleNames;
            }

            return null;
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

        public async Task<UsersServiceModel> GetAllAdmins()
        {
            var adminRoleId = dbContext.Roles.Where(r => r.Name == "Admin").FirstOrDefault().Id;
            var usersDb = dbContext.UserRoles.Where(ur => ur.RoleId == adminRoleId).ToList(); ;

            UsersServiceModel serviceModel = new UsersServiceModel();
            List<UserServiceModel> users = new List<UserServiceModel>();

            foreach (var item in usersDb)
            {
                var userDb = await GetUserById(item.UserId);
                userDb.Role = "Admin";

                users.Add(userDb);
            }

            serviceModel.Users = users;

            return serviceModel;
        }

        public async Task<UsersServiceModel> GetAllModerators()
        {
            var moderatorRoleId = dbContext.Roles.Where(r => r.Name == "Moderator").FirstOrDefault().Id;
            var usersDb = dbContext.UserRoles.Where(ur => ur.RoleId == moderatorRoleId).ToList(); ;

            UsersServiceModel serviceModel = new UsersServiceModel();
            List<UserServiceModel> users = new List<UserServiceModel>();

            foreach (var item in usersDb)
            {
                var userDb = await GetUserById(item.UserId);
                userDb.Role = "Moderator";

                users.Add(userDb);
            }

            serviceModel.Users = users;

            return serviceModel;
        }

        public async Task<bool> CheckIfUsernameIsValid(string username)
        {
            var isValidUser = dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();

            if (isValidUser == null || isValidUser == default)
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetUserIdByUsername(string username)
        {
            var userId = dbContext.Users.Where(u => u.UserName == username).FirstOrDefault().Id;

            return userId;
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
