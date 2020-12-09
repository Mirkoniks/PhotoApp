using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoApp.Web.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PhotoApp.Web.ViewModels;

namespace PhotoApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Account account = new Account("djjdavsvc", "493856735425361", "xYLUDHzVp1Zb_prW-vNs9YyX6wo");
            //Cloudinary cloudinary = new Cloudinary(account);

            //var upload = new ImageUploadParams()
            //{
            //    File = new FileDescription(@"C:\Users\Miro\Desktop\Screenshot_1.png")
            //};

            //var uploadReuslt = cloudinary.Upload(upload);

            PhotosViewModel photosViewModel = new PhotosViewModel();

            List<string> photos = new List<string>();
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607333511/tgeahxxe9dubfn4iifay.png");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325364/sample.jpg");

            photosViewModel.Photos = photos;

            return View(photosViewModel);
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
