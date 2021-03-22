using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.CloudinaryService;
using PhotoApp.Services.Models.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly PhotoAppDbContext dbContext;
        private readonly Cloudinary cloudinary;
        private readonly ICloundinaryService cloudinaryService;

        public PhotoService(PhotoAppDbContext dbContext, Cloudinary cloudinary, ICloundinaryService cloudinaryService)
        {
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<int> AddPhotoAsync(string photoLink)
        {
            var model = new Photo
            {
                Link = photoLink,
                UploadedOn = DateTime.UtcNow.Date
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

        public async Task<PhotoServiceModel> FindPhotoByIdAsync(int id)
        {
            PhotoServiceModel photoServiceModel;

            if (id != default(int))
            {
                Photo photo = dbContext.Photos.Where(p => p.PhotoId == id).FirstOrDefault();


                photoServiceModel = new PhotoServiceModel
                {
                    PhotoId = photo.PhotoId,
                    PhotoLink = photo.Link
                };

                return photoServiceModel;
            }


            photoServiceModel = new PhotoServiceModel
            {
                PhotoId = 0,
                PhotoLink = null
            };

            return photoServiceModel;
        }

        public async Task<int> GetTotalPhotosCount()
        {
            int phCount = dbContext.Photos.Count();

            return phCount;
        }

        public async Task<int> GetPhotosCountFromToday()
        {
            DateTime now = DateTime.UtcNow.Date;

            var phCount = dbContext.Photos.Where(p => p.UploadedOn.Date == now).Count();

            return phCount;
        }


        public async Task<string> GetPhotoUrl(int id)
        {
            var photoLink = dbContext.Photos.Where(p => p.PhotoId == id).FirstOrDefault().Link;

            return photoLink;
        }

        public async Task<int> GetChallangeCoverPhotoId(int id)
        {
            var photoId = dbContext.Challanges.Where(c => c.ChallangeId == id).FirstOrDefault().ChallangeCoverPhotoId;

            return photoId;
        }

        public async Task ChangeCoverPhoto(IFormFile file, string userId)
        {
            List<IFormFile> files = new List<IFormFile> { file };

            var uploadInfo = await cloudinaryService.UploadAsync(cloudinary, files);

            var picId = await AddPhotoAsync(uploadInfo.FirstOrDefault());

            dbContext.Users.Where(u => u.Id == userId).FirstOrDefault().CoverPhotoId = picId;

            await dbContext.SaveChangesAsync();
        }

        public async Task ChangeProfilePhoto(IFormFile file, string userId)
        {
            List<IFormFile> files = new List<IFormFile> { file };

            var uploadInfo = await cloudinaryService.UploadAsync(cloudinary, files);

            var picId = await AddPhotoAsync(uploadInfo.FirstOrDefault());

            dbContext.Users.Where(u => u.Id == userId).FirstOrDefault().ProfilePicId = picId;

            await dbContext.SaveChangesAsync();
        }

    }
}
