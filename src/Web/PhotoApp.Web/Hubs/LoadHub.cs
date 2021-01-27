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

        private int PhotosCount { get; set; }
        private int StepsCount { get; set; }

        public LoadHub(PhotoAppDbContext dbContext,
                        IChallangeService challangeService,
                        IPhotoService photoService
                        )
        {
            this.dbContext = dbContext;
            this.challangeService = challangeService;
            this.photoService = photoService;
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

                    PhotoViewModel photoViewModel = new PhotoViewModel
                    {
                        PhotoId = photo.PhotoId,
                        PhotoLink = photo.PhotoLink
                    };

                    photoViewModels.Add(photoViewModel);
                }
            }
            else
            {
                var photosChallanges = dbContext.PhotosChallanges.Where(c => c.ChallangeId == photos.ChallangeId).Take(step).ToList();

                for (int i = 0; i < photosChallanges.Count; i++)
                {
                    var photo = await photoService.FindPhotoByIdAsync(photosChallanges[i].PhotoId);

                    PhotoViewModel photoViewModel = new PhotoViewModel
                    {
                        PhotoId = photo.PhotoId,
                        PhotoLink = photo.PhotoLink
                    };

                    photoViewModels.Add(photoViewModel);
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
    }
}
