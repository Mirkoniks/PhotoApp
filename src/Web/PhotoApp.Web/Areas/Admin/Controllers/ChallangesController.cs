using Microsoft.AspNetCore.Mvc;
using PhotoApp.Services.ChallangeService;
using PhotoApp.Services.Models.Challange;
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
                Status = await challangeService.SetStatus(challangeServiceModel.IsOpen, challangeServiceModel.IsUpcoming)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(ChallangeViewModel challangeModel)
        {
            EditChallangeServiceModel serviceModel = new EditChallangeServiceModel();

            serviceModel.ChallangeId = challangeModel.Id;
            serviceModel.Name = challangeModel.Name;
            serviceModel.Description = challangeModel.Description;
            serviceModel.StartTime = challangeModel.StarTime;
            serviceModel.EndTime = challangeModel.EndTime;
            serviceModel.ChallangeCoverPhoto = challangeModel.ChallangeCoverPhoto;

            await challangeService.EditChallange(serviceModel);

            return Redirect("/Admin/Challanges/Challange/" + challangeModel.Id);
        }
    }
}
