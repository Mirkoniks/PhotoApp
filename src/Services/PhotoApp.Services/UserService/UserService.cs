using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.Models.Photo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly PhotoAppDbContext dbContext;

        public UserService(PhotoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
