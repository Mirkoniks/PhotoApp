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

        //NEW METHODS FOR LOADING

        public async Task LoadPhotosNew(LoadInfo model)
        {
            PhotosViewModel photosViewModel = new PhotosViewModel();
            List<PhotoViewModel> photos = new List<PhotoViewModel>();

            int photosToSend = 8;

            int userLikedPhotosCount = dbContext.UsersPhotoLikes.Where(upl => upl.UserId == model.UserId).Where(upl => upl.ChallangeId == model.ChallangeId).Count();

            int totalPhotosLeft = dbContext.PhotosChallanges.Where(c => c.ChallangeId == model.ChallangeId).Count();

            totalPhotosLeft -= userLikedPhotosCount;

            if (IsMorePhotos(model.PhotosSend, totalPhotosLeft))
            {
                totalPhotosLeft -= model.PhotosSend;

                if (photosToSend > totalPhotosLeft)
                {
                    photosToSend = totalPhotosLeft;
                }

                int skipCount = model.ActualPhotosCount;


                List<int> challangePhotosIds = dbContext.PhotosChallanges
                                                        .Where(c => c.ChallangeId == model.ChallangeId)
                                                        .Skip(skipCount)
                                                        .Take(photosToSend)
                                                        .Select(c => c.PhotoId)
                                                        .ToList();

                foreach (var item in challangePhotosIds)
                {
                    if (!dbContext.UsersPhotoLikes.Any(p => p.PhotoId == item))
                    {
                        var photoDb = await photoService.FindPhotoByIdAsync(item);

                        PhotoViewModel photo = new PhotoViewModel()
                        {
                            PhotoId = item,
                            PhotoLink = photoDb.PhotoLink
                        };

                        photos.Add(photo);
                    }
                }

                photosViewModel.Photos = photos;
                photosViewModel.ExpectMorePhotos = true;
                photosViewModel.ActualPhotosCount = photosToSend;
            }
            else
            {
                photosViewModel.ExpectMorePhotos = false;
            }

            ;

            await Clients.Caller.SendAsync(
                "GetPhotosNew",
                photosViewModel
                );

        }

        public async Task LoadAllTopPhotosNew(TopPhotosLoadInfo model)
        {
            TopPhotosModelNew modelNew = new TopPhotosModelNew();

            int totoalPhotosCount = dbContext.PhotosChallanges.Count();

            if (IsMorePhotos(model.PhotosSent, totoalPhotosCount))
            {
                List<TopPhotoModel> photos = new List<TopPhotoModel>();

                int photosToSend = 8;

                if (photosToSend > totoalPhotosCount)
                {
                    photosToSend = totoalPhotosCount;
                }

                var photosDb = dbContext.PhotosChallanges
                                      .Skip(model.PhotosSent)
                                      .Take(photosToSend)
                                      .OrderByDescending(c => c.VotesCount)
                                      .ToList();

                foreach (var item in photosDb)
                {
                    string userId = dbContext.UsersPhotos.Where(p => p.PhotoId == item.PhotoId).FirstOrDefault().UserId;

                    string username = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault().UserName;

                    TopPhotoModel photo = new TopPhotoModel()
                    {
                        ChallangeName = dbContext.Challanges.Where(c => c.ChallangeId == item.ChallangeId).FirstOrDefault().Name,
                        PhotoLink = dbContext.Photos.Where(p => p.PhotoId == item.PhotoId).FirstOrDefault().Link,
                        Username = username,
                        VotesCount = dbContext.PhotosChallanges.Where(p => p.ChallangeId == item.ChallangeId).FirstOrDefault().VotesCount
                    };

                    photos.Add(photo);
                }

                modelNew.Photos = photos;
                modelNew.ExpectMorePhotos = true;
            }
            else
            {
                modelNew.ExpectMorePhotos = false;
            }

            await Clients.Caller.SendAsync(
                "GetAllTopPhotosNew",
                modelNew
                );
        }

        public async Task LoadLatestPhotosNew(LatestPhotosLoadInfo model)
        {
            LatestPhotosModel modelNew = new LatestPhotosModel();

            int totoalPhotosCount = dbContext.PhotosChallanges.Count();

            if (IsMorePhotos(model.PhotosSent, totoalPhotosCount))
            {
                List<LatestPhotoModel> photos = new List<LatestPhotoModel>();

                int photosToSend = 8;

                if (photosToSend > totoalPhotosCount)
                {
                    photosToSend = totoalPhotosCount;
                }

                var photosDb = dbContext.PhotosChallanges
                                      .Skip(model.PhotosSent)
                                      .Take(photosToSend)
                                      .OrderByDescending(c => c.PhotoId)
                                      .ToList();

                foreach (var item in photosDb)
                {
                    string userId = dbContext.UsersPhotos.Where(p => p.PhotoId == item.PhotoId).FirstOrDefault().UserId;

                    string username = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault().UserName;

                    LatestPhotoModel photo = new LatestPhotoModel()
                    {
                        ChallangeName = dbContext.Challanges.Where(c => c.ChallangeId == item.ChallangeId).FirstOrDefault().Name,
                        PhotoLink = dbContext.Photos.Where(p => p.PhotoId == item.PhotoId).FirstOrDefault().Link,
                        Username = username,
                        VotesCount = dbContext.PhotosChallanges.Where(p => p.ChallangeId == item.ChallangeId).FirstOrDefault().VotesCount
                    };

                    photos.Add(photo);
                }

                modelNew.Photos = photos;
                modelNew.ExpectMorePhotos = true;
            }
            else
            {
                modelNew.ExpectMorePhotos = false;
            }

            await Clients.Caller.SendAsync(
                "GetLatestPhotosNew",
                modelNew
                );
        }

        public async Task ChallangeTopPhotos(ChallangeTopPhotosLoadInfo model)
        {
            ChallangeTopPhotosModel modelNew = new ChallangeTopPhotosModel();

            int totoalPhotosCount = dbContext.PhotosChallanges.Where(c => c.ChallangeId == model.ChallangeId).Count();

            if (IsMorePhotos(model.PhotosSent, totoalPhotosCount))
            {
                List<ChallangeTopPhotoModel> photos = new List<ChallangeTopPhotoModel>();

                    
                int photosToSend = 8;

                if (photosToSend > totoalPhotosCount)
                {
                    photosToSend = totoalPhotosCount;
                }

                var photosDb = dbContext.PhotosChallanges
                                        .Where(c => c.ChallangeId == model.ChallangeId)
                                        .OrderByDescending(c => c.VotesCount)
                                        .Skip(model.PhotosSent)
                                        .Take(photosToSend)
                                        .ToList();

                foreach (var item in photosDb)
                {
                    string userId = dbContext.UsersPhotos.Where(p => p.PhotoId == item.PhotoId).FirstOrDefault().UserId;

                    string username = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault().UserName;

                    ChallangeTopPhotoModel photo = new ChallangeTopPhotoModel()
                    {
                        ChallangeName = dbContext.Challanges.Where(c => c.ChallangeId == item.ChallangeId).FirstOrDefault().Name,
                        PhotoLink = dbContext.Photos.Where(p => p.PhotoId == item.PhotoId).FirstOrDefault().Link,
                        Username = username,
                        VotesCount = dbContext.PhotosChallanges.Where(p => p.ChallangeId == item.ChallangeId).FirstOrDefault().VotesCount
                    };

                    photos.Add(photo);
                }

                modelNew.Photos = photos;
                modelNew.ExpectMorePhotos = true;
            }
            else
            {
                modelNew.ExpectMorePhotos = false;
            }

            await Clients.Caller.SendAsync(
                "GetChallangeTopPhotos",
                modelNew
                );
        }


        private bool IsMorePhotos(int photosCountSent, int totalPhotos)
        {
            if (photosCountSent < totalPhotos)
            {
                return true;
            }

            return false;
        }
    }
}
