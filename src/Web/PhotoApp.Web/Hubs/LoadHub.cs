using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.PhotoService;
using PhotoApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Hubs
{
    public class LoadHub : Hub
    {
        private const int PHOTOS_STEP = 6;

        private readonly PhotoAppDbContext dbContext;
        private readonly IChallangeService challangeService;
        private readonly IPhotoService photoService;
        private readonly UserManager<PhotoAppUser> userManager;

        private int PhotosCount { get; set; }
        private int StepsCount { get; set; }

        public LoadHub(PhotoAppDbContext dbContext,
                        IChallangeService challangeService,
                        IPhotoService photoService,
                        UserManager<PhotoAppUser> userManager
                        )
        {
            this.dbContext = dbContext;
            this.challangeService = challangeService;
            this.photoService = photoService;
            this.userManager = userManager;
        }

        public async Task LoadPhotos(LoadInfo photos)
        {

            List<PhotoViewModel> photoViewModels = new List<PhotoViewModel>();

            StepsCount = photos.StepsCount;
            PhotosCount = photos.TotalPhotos;

            if (StepsCount == 0)
            {
                PhotosCount = dbContext.PhotosChallanges.Where(c => c.ChallangeId == photos.ChallangeId).Count();
            }

            if (PhotosCount == 0)
            {
                return;
            }

            int step = PHOTOS_STEP;

            if (PhotosCount < step)
            {
                step = PhotosCount;
            }

            if (StepsCount != 0)
            {
                var photosChallanges = dbContext
                                       .PhotosChallanges
                                       .Where(c => c.ChallangeId == photos.ChallangeId)
                                       .Skip(StepsCount)
                                       .Take(step)
                                       .ToList();


                for (int i = 0; i < photosChallanges.Count; i++)
                {
                    var photo = await photoService.FindPhotoByIdAsync(photosChallanges[i].PhotoId);

                    var isLiked = (dbContext.UsersPhotoLikes.Any(p => p.PhotoId == photo.PhotoId)) && (dbContext.UsersPhotoLikes.Any(u => u.UserId == photos.UserId));

                    if (!isLiked)
                    {
                        PhotoViewModel photoViewModel = new PhotoViewModel
                        {
                            PhotoId = photo.PhotoId,
                            PhotoLink = photo.PhotoLink
                        };

                        photoViewModels.Add(photoViewModel);
                    }
                }
            }
            else
            {
                var photosChallanges = dbContext.PhotosChallanges.Where(c => c.ChallangeId == photos.ChallangeId).Take(step).ToList();

                for (int i = 0; i < photosChallanges.Count; i++)
                {
                    var photo = await photoService.FindPhotoByIdAsync(photosChallanges[i].PhotoId);

                    var isLiked = (dbContext.UsersPhotoLikes.Any(p => p.PhotoId == photo.PhotoId)) && (dbContext.UsersPhotoLikes.Any(u => u.UserId == photos.UserId));

                    if (!isLiked)
                    {
                        PhotoViewModel photoViewModel = new PhotoViewModel
                        {
                            PhotoId = photo.PhotoId,
                            PhotoLink = photo.PhotoLink
                        };

                        photoViewModels.Add(photoViewModel);
                    };
                }
            }

            StepsCount += step;

            PhotosCount -= step;

            PhotosViewModel1 photosViewModel = new PhotosViewModel1
            {
                Photos = photoViewModels,
                PhotosCount = PhotosCount,
                StepsCount = StepsCount
            };

             await Clients.Caller.SendAsync(
                 "GetPhotos",
                  photosViewModel
                 );
        }

        public async Task LoadTopPhotos(TopPhotosModel model)
        {
            int step = 6;
            int photosCount = dbContext.PhotosChallanges.Where(c => c.ChallangeId == model.ChallangeId).Count();
            int photosToGet = photosCount - model.PhotosSend;

            if (photosToGet < 0)
            {
                return;
            }
            else if (photosToGet < step)
            {
                step = photosToGet;
            }

            var photos = dbContext.PhotosChallanges.Where(c => c.ChallangeId == model.ChallangeId).OrderByDescending(v => v.VotesCount).Skip(model.PhotosSend).Take(step).ToList();

            TopPhotosModel topPhotosModel = new TopPhotosModel();
            List<TopPhotoModel> photoModels = new List<TopPhotoModel>();

            foreach (var photo in photos)
            {
                TopPhotoModel topPhotoModel = new TopPhotoModel();
                string username = userManager.FindByIdAsync(dbContext.UsersPhotos.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().UserId).Result.UserName;

                topPhotoModel.VotesCount = photo.VotesCount;
                topPhotoModel.PhotoLink = dbContext.Photos.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().Link;
                topPhotoModel.Username = username;

                photoModels.Add(topPhotoModel);
            }

            topPhotosModel.Photos = photoModels;

            await Clients.Caller.SendAsync(
                "GetTopPhotos",
                 topPhotosModel
                );
        }
    }
}
