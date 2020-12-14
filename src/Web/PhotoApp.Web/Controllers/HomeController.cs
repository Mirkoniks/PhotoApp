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
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325378/samples/landscapes/nature-mountains.jpg");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325375/samples/landscapes/beach-boat.jpg");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325374/samples/landscapes/architecture-signs.jpg");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325371/samples/landscapes/girl-urban-view.jpg");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325379/samples/animals/kitten-playing.gif");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325373/samples/animals/three-dogs.jpg");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325369/samples/animals/reindeer.jpg");
            photos.Add("https://res.cloudinary.com/djjdavsvc/image/upload/v1607325368/samples/animals/cat.jpg");


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
