using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoApp.Data;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.CloudinaryService;
using PhotoApp.Services.Models.Challange;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.UserService;
using PhotoApp.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Controllers
{
    [Authorize]
    public class ChallangesController : Controller
    {
        private readonly IChallangeService challangeService;
        private readonly ICloundinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;
        private readonly IPhotoService photoService;
        private readonly UserManager<PhotoAppUser> userManager;
        private readonly IUserService userService;

        public ChallangesController(IChallangeService challangeService,
                                     ICloundinaryService cloudinaryService,
                                     Cloudinary cloudinary,
                                     IPhotoService photoService,
                                     UserManager<PhotoAppUser> userManager,
                                     IUserService userService)
        {
            this.challangeService = challangeService;
            this.cloudinaryService = cloudinaryService;
            this.cloudinary = cloudinary;
            this.photoService = photoService;
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> OpenAsync()
        {
            AllChallangesServiceModel serviceModel = await challangeService.GetAllOpenChallanges();

            OpenChallangesViewModel viewModel = new OpenChallangesViewModel();
            List<ChallangeViewModel> challangesList = new List<ChallangeViewModel>();

            foreach (var item in serviceModel.Challanges)
            {
                string photoLink = "";

                var photo = await challangeService.FirstTopPhotosFromChallange(item.Id, 1);

                if (item.ChallangeCoverPhotoLink != null)
                {
                    photoLink = item.ChallangeCoverPhotoLink;
                }
                else if (photo.Photos.FirstOrDefault() == null)
                {
                    photoLink = "https://www.ecpgr.cgiar.org/fileadmin/templates/ecpgr.org/Assets/images/No_Image_Available.jpg";
                }
                else
                {
                    photoLink = photo.Photos.FirstOrDefault().PhotoLink;
                }

                ChallangeViewModel model = new ChallangeViewModel()
                {

                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime.Date,
                    EndTime = item.EndTime.Date,
                    CoverPhotoLink = photoLink
                };

                challangesList.Add(model);
            }
            viewModel.Challanges = challangesList;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ClosedAsync()
        {
            AllChallangesServiceModel serviceModel = await challangeService.GetAllClosedCallanges();

            OpenChallangesViewModel viewModel = new OpenChallangesViewModel();
            List<ChallangeViewModel> challangesList = new List<ChallangeViewModel>();

            foreach (var item in serviceModel.Challanges)
            {
                string photoLink = "";

                var photo = await challangeService.FirstTopPhotosFromChallange(item.Id, 1);

                if (item.ChallangeCoverPhotoLink != null)
                {
                    photoLink = item.ChallangeCoverPhotoLink;
                }
                else if (photo.Photos.FirstOrDefault() == null)
                {
                    photoLink = "https://www.ecpgr.cgiar.org/fileadmin/templates/ecpgr.org/Assets/images/No_Image_Available.jpg";
                }
                else
                {
                    photoLink = photo.Photos.FirstOrDefault().PhotoLink;
                }

                ChallangeViewModel model = new ChallangeViewModel()
                {

                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime.Date,
                    EndTime = item.EndTime.Date,
                    CoverPhotoLink = photoLink
                };

                challangesList.Add(model);
            }
            viewModel.Challanges = challangesList;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpcomingAsync()
        {
            AllChallangesServiceModel serviceModel = await challangeService.GetAllUpcomingChallanges();

            OpenChallangesViewModel viewModel = new OpenChallangesViewModel();
            List<ChallangeViewModel> challangesList = new List<ChallangeViewModel>();

            foreach (var item in serviceModel.Challanges)
            {
                string photoLink = "";

                var photo = await challangeService.FirstTopPhotosFromChallange(item.Id, 1);

                if (item.ChallangeCoverPhotoLink != null)
                {
                    photoLink = item.ChallangeCoverPhotoLink;
                }
                else if (photo.Photos.FirstOrDefault() != null)
                {
                    photoLink = photo.Photos.FirstOrDefault().PhotoLink;
                }
                else
                {
                    photoLink = "https://www.ecpgr.cgiar.org/fileadmin/templates/ecpgr.org/Assets/images/No_Image_Available.jpg";
                }

                ChallangeViewModel model = new ChallangeViewModel()
                {

                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime.Date,
                    EndTime = item.EndTime.Date,
                    CoverPhotoLink = photoLink
                };

                challangesList.Add(model);
            }
            viewModel.Challanges = challangesList;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateConfirmAsync(CreateChallangeModel model)
        {
            CreateChallangeServiceModel serviceModel = new CreateChallangeServiceModel()
            {
                Name = model.Name,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            var photoLink = await cloudinaryService.UploadAsync(cloudinary, model.Photos);
            int photoId = await photoService.AddPhotoAsync(photoLink.FirstOrDefault());
            int challangeId = await challangeService.CreateChallangeAsync(serviceModel);

            await challangeService.SetChallangeCoverPhoto(challangeId, photoId);

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ChallangeAsync(int id)
        {
            string photoLink;
            var photo = await challangeService.FirstTopPhotosFromChallange(id, 1);

            if (photo.Photos.FirstOrDefault() == null)
            {
                photoLink = "https://www.ecpgr.cgiar.org/fileadmin/templates/ecpgr.org/Assets/images/No_Image_Available.jpg";
            }
            else
            {
                photoLink = photo.Photos.FirstOrDefault().PhotoLink;
            }
            ChallangeServiceModel serviceModel = await challangeService.FindChallangeById(id);

            ChallangeViewModel model = new ChallangeViewModel()
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                StartTime = serviceModel.StartTime,
                EndTime = serviceModel.EndTime,
                UserId = userManager.GetUserId(this.User),
                TopPhotoLink = photoLink,
                IsUpcoming = serviceModel.IsUpcoming,
                IsOpen = serviceModel.IsOpen
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files, int challangeId)
        {
            var photoLinks = await cloudinaryService.UploadAsync(this.cloudinary, files);
            string userId = userManager.GetUserId(this.User);

            foreach (var photo in photoLinks)
            {
                int photoId = await photoService.AddPhotoAsync(photo);

                await userService.AssignUserToPhotoAsync(userId, photoId);

                await challangeService.AddPhotoToChallange(photoId, challangeId);
            }


            return Redirect("/Challanges/Challange/" + challangeId);

        }

        [HttpGet]

        public async Task<IActionResult> ChallangePhotosAsync(int id)
        {
            TopPhotosServiceModel serviceModel = await challangeService.FirstTopPhotosFromChallange(id, 10);

            TopPhotosViewModel viewModel = new TopPhotosViewModel();
            List<TopPhotoViewModel> list = new List<TopPhotoViewModel>();

            foreach (var item in serviceModel.Photos)
            {
                TopPhotoViewModel topPhoto = new TopPhotoViewModel();

                var username = await userManager.FindByIdAsync(item.UserId);

                topPhoto.PhotoLink = item.PhotoLink;
                topPhoto.Username = username.UserName;
                topPhoto.VotesCount = item.VotesCount;

                list.Add(topPhoto);
            }

            viewModel.ChallangeId = serviceModel.ChallangeId;
            viewModel.Photos = list;

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> TopAsync()
        {
            TopPhotosServiceModel serviceModel = await challangeService.GetTopPhotos(10);

            TopPhotosViewModel viewModel = new TopPhotosViewModel();
            List<TopPhotoViewModel> list = new List<TopPhotoViewModel>();

            foreach (var item in serviceModel.Photos)
            {
                TopPhotoViewModel topPhoto = new TopPhotoViewModel();

                var username = await userManager.FindByIdAsync(item.UserId);

                topPhoto.PhotoLink = item.PhotoLink;
                topPhoto.Username = username.UserName;
                topPhoto.VotesCount = item.VotesCount;
                topPhoto.ChallangeName = item.ChallangeName;

                list.Add(topPhoto);
            }

            viewModel.ChallangeId = serviceModel.ChallangeId;
            viewModel.Photos = list;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LatestAsync()
        {
            TopPhotosServiceModel serviceModel = await challangeService.GetLatestPhotos(10);

            TopPhotosViewModel viewModel = new TopPhotosViewModel();
            List<TopPhotoViewModel> list = new List<TopPhotoViewModel>();

            foreach (var item in serviceModel.Photos)
            {
                TopPhotoViewModel topPhoto = new TopPhotoViewModel();

                var username = await userManager.FindByIdAsync(item.UserId);

                topPhoto.PhotoLink = item.PhotoLink;
                topPhoto.Username = username.UserName;
                topPhoto.VotesCount = item.VotesCount;
                topPhoto.ChallangeName = item.ChallangeName;

                list.Add(topPhoto);
            }

            viewModel.ChallangeId = serviceModel.ChallangeId;
            viewModel.Photos = list;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            EditChallangeViewModel viewModel = new EditChallangeViewModel();

            var challange = await challangeService.FindChallangeById(id);

            viewModel.Name = challange.Name;
            viewModel.Description = challange.Description;
            viewModel.StartTime = challange.StartTime;
            viewModel.EndTime = challange.EndTime;
            viewModel.ChallangeId = id;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirmAsync(EditChallangeViewModel challangeViewModel)
        {

            EditChallangeServiceModel serviceModel = new EditChallangeServiceModel();

            serviceModel.ChallangeId = challangeViewModel.ChallangeId;
            serviceModel.Name = challangeViewModel.Name;
            serviceModel.Description = challangeViewModel.Description;
            serviceModel.StartTime = challangeViewModel.StartTime;
            serviceModel.EndTime = challangeViewModel.EndTime;
            serviceModel.ChallangeCoverPhoto = challangeViewModel.ChallangeCoverPhoto;

            await challangeService.EditChallange(serviceModel);

            return Redirect("/Challanges/Challange/" + challangeViewModel.ChallangeId);
        }
    }
}
