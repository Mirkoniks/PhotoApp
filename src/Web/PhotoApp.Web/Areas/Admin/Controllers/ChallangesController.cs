using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.CloudinaryService;
using PhotoApp.Services.Models.Challange;
using PhotoApp.Services.PhotoService;
using PhotoApp.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChallangesController : Controller
    {
        private readonly IChallangeService challangeService;
        private readonly IPhotoService photoService;
        private readonly Cloudinary cloudinary;
        private readonly ICloundinaryService cloudinaryService;

        public ChallangesController(IChallangeService challangeService,
                                    IPhotoService photoService,
                                    Cloudinary cloudinary,
                                    ICloundinaryService cloudinaryService)
        {
            this.challangeService = challangeService;
            this.photoService = photoService;
            this.cloudinary = cloudinary;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Ongoing()
        {
            AllChalangesViewModel allChalangesViewModel = new AllChalangesViewModel();
            List<ChallangeViewModel> challangeViewModels = new List<ChallangeViewModel>();

            var challanges = await challangeService.AdminGetAllOpenChallanges();

            foreach (var item in challanges.Challanges)
            {
                ChallangeViewModel model = new ChallangeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StarTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = await challangeService.SetStatus(item.IsOpen, item.IsUpcoming),
                };

                challangeViewModels.Add(model);
            }

            allChalangesViewModel.Challanges = challangeViewModels;

            return View(allChalangesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Upcoming()
        {
            AllChalangesViewModel allChalangesViewModel = new AllChalangesViewModel();
            List<ChallangeViewModel> challangeViewModels = new List<ChallangeViewModel>();

            var challanges = await challangeService.AdminGetAllUpcomingChallanges();

            foreach (var item in challanges.Challanges)
            {
                ChallangeViewModel model = new ChallangeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StarTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = await challangeService.SetStatus(item.IsOpen, item.IsUpcoming),
                };

                challangeViewModels.Add(model);
            }

            allChalangesViewModel.Challanges = challangeViewModels;

            return View(allChalangesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Closed()
        {
            AllChalangesViewModel allChalangesViewModel = new AllChalangesViewModel();
            List<ChallangeViewModel> challangeViewModels = new List<ChallangeViewModel>();

            var challanges = await challangeService.AdminGetAllClosedChallanges();

            foreach (var item in challanges.Challanges)
            {
                ChallangeViewModel model = new ChallangeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StarTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = await challangeService.SetStatus(item.IsOpen, item.IsUpcoming),
                };

                challangeViewModels.Add(model);
            }

            allChalangesViewModel.Challanges = challangeViewModels;

            return View(allChalangesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            AllChalangesViewModel allChalangesViewModel = new AllChalangesViewModel();
            List<ChallangeViewModel> challangeViewModels = new List<ChallangeViewModel>();

            var challanges =  await challangeService.AdminGetAllChallanges();

            foreach (var item in challanges.Challanges)
            {
                ChallangeViewModel model = new ChallangeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    StarTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status =  await challangeService.SetStatus(item.IsOpen, item.IsUpcoming),
                };

                challangeViewModels.Add(model);
            }

            allChalangesViewModel.Challanges = challangeViewModels;

            return View(allChalangesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Challange(int id)
        {
            var challangeServiceModel =  await challangeService.GetChallangeById(id);

            ChallangeViewModel viewModel = new ChallangeViewModel()
            {
                Id = challangeServiceModel.Id,
                Name = challangeServiceModel.Name,
                Description = challangeServiceModel.Description,
                StarTime = challangeServiceModel.StartTime,
                EndTime = challangeServiceModel.EndTime,
                Status = await challangeService.SetStatus(challangeServiceModel.IsOpen, challangeServiceModel.IsUpcoming),
                CoverPhotoLink = await photoService.GetPhotoUrl( await photoService.GetChallangeCoverPhotoId(id))               
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChallangeViewModel challangeModel)
        {
            EditChallangeServiceModel serviceModel = new EditChallangeServiceModel();

            serviceModel.ChallangeId = challangeModel.Id;
            serviceModel.Name = challangeModel.Name;
            serviceModel.Description = challangeModel.Description;
            serviceModel.StartTime = challangeModel.StarTime;
            serviceModel.EndTime = challangeModel.EndTime;

            if (challangeModel.ChallangeCoverPhoto != null)
            {

                var photoLink = await cloudinaryService.UploadAsync(cloudinary, challangeModel.ChallangeCoverPhoto);
                int photoId = await photoService.AddPhotoAsync(photoLink.FirstOrDefault());
                await challangeService.SetChallangeCoverPhoto(challangeModel.Id, photoId);

            }

            await challangeService.EditChallange(serviceModel);

            return Redirect("/Admin/Challanges/Challange/" + challangeModel.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateConfirm(CreateChallangeModel model)
        {
            CreateChallangeServiceModel serviceModel = new CreateChallangeServiceModel()
            {
                Name = model.Name,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            int challangeId = await challangeService.CreateChallangeAsync(serviceModel);

            if (model.Photos != null)
            {

                var photoLink = await cloudinaryService.UploadAsync(cloudinary, model.Photos);
                int photoId = await photoService.AddPhotoAsync(photoLink.FirstOrDefault());
                await challangeService.SetChallangeCoverPhoto(challangeId, photoId);

            }

            return RedirectToAction("All");
        }
    }
}
