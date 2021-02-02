using CloudinaryDotNet;
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

        public IActionResult Open()
        {
            AllChallangesServiceModel serviceModel = challangeService.GetFisrtChallanges(9);

            OpenChallangesViewModel viewModel = new OpenChallangesViewModel();
            List<ChallangeViewModel> challangesList = new List<ChallangeViewModel>();

            foreach (var item in serviceModel.Challanges)
            {
                ChallangeViewModel model = new ChallangeViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
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

           await challangeService.CreateChallangeAsync(serviceModel);

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ChallangeAsync(int id)
        {
            ChallangeServiceModel serviceModel = await challangeService.FindChallangeById(id);

            ChallangeViewModel model = new ChallangeViewModel()
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                StartTime = serviceModel.StartTime,
                EndTime = serviceModel.EndTime,
                UserId = userManager.GetUserId(this.User)
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
    }
}
