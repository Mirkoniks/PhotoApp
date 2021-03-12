using Microsoft.AspNetCore.Mvc;
using PhotoApp.Services.ChallangeService;
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

        public ChallangesController(IChallangeService challangeService
                                    )
        {
            this.challangeService = challangeService;
        }

        public async Task<IActionResult> Ongoing()
        {
            return View();
        }

        public async Task<IActionResult> Upcoming()
        {
            return View();
        }

        public async Task<IActionResult> Closed()
        {
            return View();
        }

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
                    Status =  await challangeService.SetStatus(item.IsOpen, item.IsUpcoming)
                };

                challangeViewModels.Add(model);
            }

            allChalangesViewModel.Challanges = challangeViewModels;

            return View(allChalangesViewModel);
        }

        public async Task<IActionResult> Challange(int id)
        {
            return View();
        }
    }
}
