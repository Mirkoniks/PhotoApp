using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoApp.Data.Models;
using PhotoApp.Services.ChallangeService;
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
        private readonly SignInManager<PhotoAppUser> signInManager;

        public HomeController(ILogger<HomeController> logger,
                              ICloundinaryService cloudinaryService,
                              Cloudinary cloudinary,
                              UserManager<PhotoAppUser> userManager,
                              IPhotoService photoService,
                              IUserService userService,
                              IChallangeService challangeService,
                              SignInManager<PhotoAppUser> signInManager)
        {
            _logger = logger;
            this.cloudinaryService = cloudinaryService;
            this.cloudinary = cloudinary;
            this.userManager = userManager;
            this.photoService = photoService;
            this.userService = userService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> HomeAsync()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files, int id)
        {
            var photoLinks = await cloudinaryService.UploadAsync(this.cloudinary, files);
            string userId = userManager.GetUserId(this.User);

            foreach (var photo in photoLinks)
            {
                int photoId = await photoService.AddPhotoAsync(photo);

                await userService.AssignUserToPhotoAsync(userId, photoId);
            }

            return Redirect("/Challanges/Challange/1");

        }

        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            var isValid = await userService.CheckIfUsernameIsValid(q);

            if (isValid)
            {
                return Redirect("/User/Profile/Profile?username=" + q);
            }

            return Redirect("/Home/Error");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
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
