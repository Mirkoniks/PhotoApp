using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.Models.Photo;
using PhotoApp.Services.NotificationService;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.UserService;
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
        private readonly IUserService userService;
        private readonly IPhotoService photoService;
        private readonly INotificationService notificationService;

        public UserController(IChallangeService challangeService,
                              UserManager<PhotoAppUser> userManager,
                              IUserService userService,
                              IPhotoService photoService,
                              INotificationService notificationService)
        {
            this.challangeService = challangeService;
            this.userManager = userManager;
            this.userService = userService;
            this.photoService = photoService;
            this.notificationService = notificationService;
        }

        public async Task<IActionResult> MyPhotosAsync()
        {
            var userId = userManager.GetUserId(this.User);
            MyPhotosViewModel myPhotosViewModel = new MyPhotosViewModel();
            List<MyPhotoViewModel> list = new List<MyPhotoViewModel>();

            var serviceModel = await challangeService.GetUserPhotos(userId);

            if (serviceModel != null && (serviceModel.Photo.Count()) > 0)
            {

                foreach (var item in serviceModel.Photo)
                {
                    MyPhotoViewModel photoServiceModel = new MyPhotoViewModel
                    {
                        PhotoLink = item.PhotoLink
                    };

                    list.Add(photoServiceModel);
                }
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

            if (serviceModel.Photos != null)
            {
                if (serviceModel.Photos.Count() > 0)
                {
                    foreach (var item in serviceModel.Photos)
                    {
                        LikedPhotoViewModel photoServiceModel = new LikedPhotoViewModel
                        {
                            PhotoLink = item.PhotoLink
                        };

                        list.Add(photoServiceModel);
                    }
                }
            }

            myPhotosViewModel.Photos = list;

            return View(myPhotosViewModel);
        }

        public async Task<IActionResult> Profile(string username)
        {
            var userId = await userService.GetUserIdByUsername(username);
            var user = await userService.GetUserById(userId);


            UserViewModel userViewModel = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                CoverPhotoLink = await photoService.GetPhotoUrl(user.CoverPhotoId),
                PhotosCount = await userService.GetUserPhotosCount(userId),
                ProfilePicLink = await photoService.GetPhotoUrl(user.ProfilePicId),
                UserId = user.Id
            };

            var photos = await userService.GetUserPhotos(userId);

            List<PhotoViewModel> photosLinks = new List<PhotoViewModel>();

            foreach (var item in photos)
            {
                PhotoViewModel photo = new PhotoViewModel
                {
                    Id = item.Id,
                    Link = item.Link
                };

                photosLinks.Add(photo);
            }

            if (photos == null)
            {
                PhotoViewModel photo = new PhotoViewModel
                {
                    Id = 0,
                    Link = ""
                };
                photosLinks.Add(photo);

                return View(userViewModel);
            }

            userViewModel.PhotoLinks = photosLinks;

            return View(userViewModel);
        }

        public async Task<IActionResult> Notifications()
        {
            var userId = userManager.GetUserId(this.User);

            var nf = await notificationService.GetUserNotifcations(userId);

            NotificationsViewModel viewModel = new NotificationsViewModel();
            List<NotificationViewModel> notifications = new List<NotificationViewModel>();

            foreach (var item in nf)
            {
                NotificationViewModel notificationViewModel = new NotificationViewModel()
                {
                    ChallengeId = item.ChallangeId,
                    ChallengeName = item.ChallangeName,
                    WinnerPhotoLink = await photoService.GetTopChallangePhotoLink(item.ChallangeId),
                    NotificatonId = item.NotificationId
                };

                notifications.Add(notificationViewModel);
            }

            viewModel.Notifications = notifications;

            return View(viewModel);
        }

        public async Task<IActionResult> Discard(int id)
        {
            await notificationService.DismisNotification(id);

            return Redirect("/");
        }

        public async Task<IActionResult> ChangeProflePhoto(IFormFile file)
        {
            var userId = userManager.GetUserId(this.User);

            await photoService.ChangeProfilePhoto(file, userId);

            return Redirect("/Identity/Account/Manage");
        }

        public async Task<IActionResult> ChaneCoverPhoto(IFormFile file)
        {
            var userId = userManager.GetUserId(this.User);

            await photoService.ChangeCoverPhoto(file, userId);

            return Redirect("/Identity/Account/Manage");

        }
    }
}
