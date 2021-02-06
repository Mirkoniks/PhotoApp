using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.Models.Photo;
using PhotoApp.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IChallangeService challangeService;
        private readonly UserManager<PhotoAppUser> userManager;

        public UserController(IChallangeService challangeService, UserManager<PhotoAppUser> userManager)
        {
            this.challangeService = challangeService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> MyPhotosAsync()
        {
           var userId =  userManager.GetUserId(this.User);

            var serviceModel = await challangeService.GetUserPhotos(userId);

            MyPhotosViewModel myPhotosViewModel = new MyPhotosViewModel();
            List<MyPhotoViewModel> list = new List<MyPhotoViewModel>();

            foreach (var item in serviceModel.Photo)
            {
                MyPhotoViewModel photoServiceModel = new MyPhotoViewModel
                {
                    PhotoLink = item.PhotoLink
                };

                list.Add(photoServiceModel);
            }

            myPhotosViewModel.Photos = list;

            return View(myPhotosViewModel);
        }

        public async Task<IActionResult> LikedPhotosAsync()
        {
            var userId = userManager.GetUserId(this.User);

            var serviceModel = await challangeService.GetUserLikedPhotos(userId);

            LikedPhotosViewModel myPhotosViewModel = new LikedPhotosViewModel();
            List<LikedPhotoViewModel> list = new List<LikedPhotoViewModel>();

            foreach (var item in serviceModel.Photos)
            {
                LikedPhotoViewModel photoServiceModel = new LikedPhotoViewModel
                {
                    PhotoLink = item.PhotoLink
                };

                list.Add(photoServiceModel);
            }

            myPhotosViewModel.Photos = list;

            return View(myPhotosViewModel);
        }
    }
}
