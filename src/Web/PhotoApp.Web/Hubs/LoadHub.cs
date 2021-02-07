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
            int photosCount;

            if (photos.PhotosSend == 0)
            {
                photosCount = dbContext.PhotosChallanges.Where(c => c.ChallangeId == photos.ChallangeId).Count();
            }
            else
            {
                photosCount = photos.TotalPhotos;
            }
            int step = 6;
            int photosToGet = photosCount - photos.PhotosSend;

            if (photosToGet < 0)
            {
                return;
            }
            else if (photosToGet < step)
            {
                step = photosToGet;
            }

            var photosChallanges = dbContext.PhotosChallanges.Where(c => c.ChallangeId == photos.ChallangeId).Skip(photos.PhotosSend).Take(step).ToList();


            for (int i = 0; i < photosChallanges.Count; i++)
            {
                var photo = await photoService.FindPhotoByIdAsync(photosChallanges[i].PhotoId);

                //var isLiked = (dbContext.UsersPhotoLikes.Any(p => p.PhotoId == photo.PhotoId)) && (dbContext.UsersPhotoLikes.Any(u => u.UserId == photos.UserId));

                var photosLikes = dbContext.UsersPhotoLikes.Where(p => p.PhotoId == photosChallanges[i].PhotoId).FirstOrDefault();

                bool isLiked = false;

                if (photosLikes != null)
                {
                    isLiked = photosLikes.UserId == photos.UserId;
                    photosCount--;
                }

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
            PhotosViewModel1 photosViewModel = new PhotosViewModel1
            {
                TotalPhotos = photosCount,
                Photos = photoViewModels,
                PhotosCount = PhotosCount,
                StepsCount = StepsCount,
                IsOpen = dbContext.Challanges.Where(c => c.ChallangeId == photos.ChallangeId).FirstOrDefault().IsOpen,
                IsUpcomig = dbContext.Challanges.Where(c => c.ChallangeId == photos.ChallangeId).FirstOrDefault().IsUpcoming,
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
            topPhotosModel.PhotosCount = photosCount;

            await Clients.Caller.SendAsync(
                "GetTopPhotos",
                 topPhotosModel
                );
        }

        public async Task LoadAllTopPhotos(TopPhotosModel model)
        {
            int step = 10;
            int photosCount = dbContext.PhotosChallanges.Count();
            int photosToGet = photosCount - model.PhotosSend;

            if (photosToGet < 0)
            {
                return;
            }
            else if (photosToGet < step)
            {
                step = photosToGet;
            }

            var photos = dbContext.PhotosChallanges.OrderByDescending(v => v.VotesCount).Skip(model.PhotosSend).Take(step).ToList();

            TopPhotosModel topPhotosModel = new TopPhotosModel();
            List<TopPhotoModel> photoModels = new List<TopPhotoModel>();

            foreach (var photo in photos)
            {
                TopPhotoModel topPhotoModel = new TopPhotoModel();
                string username = userManager.FindByIdAsync(dbContext.UsersPhotos.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().UserId).Result.UserName;
                var challangeId = dbContext.PhotosChallanges.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().ChallangeId;
                string challangeName = dbContext.Challanges.Where(c => c.ChallangeId == challangeId).FirstOrDefault().Name;

                topPhotoModel.VotesCount = photo.VotesCount;
                topPhotoModel.PhotoLink = dbContext.Photos.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().Link;
                topPhotoModel.Username = username;
                topPhotoModel.ChallangeName = challangeName;

                photoModels.Add(topPhotoModel);
            }

            topPhotosModel.Photos = photoModels;
            topPhotosModel.PhotosCount = photosCount;

            await Clients.Caller.SendAsync(
                "GetAllTopPhotos",
                 topPhotosModel
                );
        }

        public async Task LoadLatestPhotos(TopPhotosModel model)
        {
            int step = 10;
            int photosCount = dbContext.PhotosChallanges.Count();
            int photosToGet = photosCount - model.PhotosSend;

            if (photosToGet < 0)
            {
                return;
            }
            else if (photosToGet < step)
            {
                step = photosToGet;
            }

            var photos = dbContext.PhotosChallanges.OrderByDescending(p => p.PhotoId).Skip(model.PhotosSend).Take(step).ToList();

            TopPhotosModel topPhotosModel = new TopPhotosModel();
            List<TopPhotoModel> photoModels = new List<TopPhotoModel>();

            foreach (var photo in photos)
            {
                TopPhotoModel topPhotoModel = new TopPhotoModel();
                string username = userManager.FindByIdAsync(dbContext.UsersPhotos.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().UserId).Result.UserName;
                var challangeId = dbContext.PhotosChallanges.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().ChallangeId;
                string challangeName = dbContext.Challanges.Where(c => c.ChallangeId == challangeId).FirstOrDefault().Name;

                topPhotoModel.VotesCount = photo.VotesCount;
                topPhotoModel.PhotoLink = dbContext.Photos.Where(p => p.PhotoId == photo.PhotoId).FirstOrDefault().Link;
                topPhotoModel.Username = username;
                topPhotoModel.ChallangeName = challangeName;

                photoModels.Add(topPhotoModel);
            }

            topPhotosModel.Photos = photoModels;
            topPhotosModel.PhotosCount = photosCount;

            await Clients.Caller.SendAsync(
                "GetLatestPhotos",
                 topPhotosModel
                );
        }
    }
}
