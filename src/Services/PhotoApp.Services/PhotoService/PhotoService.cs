using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.Models.Photo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly PhotoAppDbContext dbContext;

        public PhotoService(PhotoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> AddPhotoAsync(string photoLink)
        {
            var model = new Photo
            {
                Link = photoLink,
            };

            await dbContext.Photos.AddAsync(model);

            await dbContext.SaveChangesAsync();

            int photoId = model.PhotoId;

            return photoId;
        }

        public async Task AssingPhotoToChallangeAsync(AssignPhotoToChallangeServiceModel serviceModel)
        {
            PhotoChallange photoChallange = new PhotoChallange
            {
                PhotoId = serviceModel.PhotoId,
                ChallangeId = serviceModel.ChallangeId
            };

            await dbContext.PhotosChallanges.AddAsync(photoChallange);
            await dbContext.SaveChangesAsync();
        }

        public async Task FindPhotoByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
