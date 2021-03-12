using Microsoft.AspNetCore.Mvc;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.UserService;
using PhotoApp.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IPhotoService photoService;
        private readonly IUserService userService;
        private readonly IChallangeService challangeService;

        public AdminController(IPhotoService photoService,
                               IUserService userService,
                               IChallangeService challangeService)
        {
            this.photoService = photoService;
            this.userService = userService;
            this.challangeService = challangeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel viewModel = new IndexViewModel()
            {
                TotalPhotosCount = await photoService.GetTotalPhotosCount(),
                NewPhotosCount = await photoService.GetPhotosCountFromToday(),
                TotalUsersCount = await userService.GetTotalUsersCount(),
                NewUsersCount = await userService.GetUsersCountFromToday(),
                NewReportsCount = 0,
                UpcomingChallangesCount = await challangeService.GetUpcomigChallangesCount(),
                OpenChallangeCount = await challangeService.GetOpenChallangesCount(),
                ClosedChallangesCount = await challangeService.GetClosedChallangesCount()
            };


            return View(viewModel);
        }

        public IActionResult Challanges()
        {
            return View();
        }
    }
}
