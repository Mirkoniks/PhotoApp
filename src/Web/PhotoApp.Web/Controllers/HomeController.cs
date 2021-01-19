using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoApp.Data.Models;
using PhotoApp.Services.CloudinaryService;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.UserService;
using PhotoApp.Web.Models;
using PhotoApp.Web.ViewModels;

namespace PhotoApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICloundinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;
        private readonly UserManager<PhotoAppUser> userManager;
        private readonly IPhotoService photoService;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger,
                              ICloundinaryService cloudinaryService,
                              Cloudinary cloudinary,
                              UserManager<PhotoAppUser> userManager,
                              IPhotoService photoService,
                              IUserService userService)
        {
            _logger = logger;
            this.cloudinaryService = cloudinaryService;
            this.cloudinary = cloudinary;
            this.userManager = userManager;
            this.photoService = photoService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            ViewModels.PhotosViewModel photosViewModel = new ViewModels.PhotosViewModel();


            List<string> photos = new List<string>();
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607333511/tgeahxxe9dubfn4iifay.png");

            photosViewModel.Photos = photos;
            photosViewModel.UserId = userManager.GetUserId(this.User);

            return View(photosViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files, int id)
        {
            var photoLinks = await cloudinaryService.UploadAsync(this.cloudinary, files);
            string userId = userManager.GetUserId(this.User);

            foreach (var photo in photoLinks)
            {
               int photoId = await photoService.AddPhotoAsync(photo);

              await userService.AssignUserToPhotoAsync(userId ,photoId);
            }

            return Redirect("/Challanges/Challange/1");

        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
