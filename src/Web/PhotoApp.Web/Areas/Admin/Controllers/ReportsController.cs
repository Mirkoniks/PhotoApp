using Microsoft.AspNetCore.Mvc;
using PhotoApp.Services.PhotoService;
using PhotoApp.Services.ReportService;
using PhotoApp.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;
        private readonly IPhotoService photoService;

        public ReportsController(IReportService reportService,
                                 IPhotoService photoService)
        {
            this.reportService = reportService;
            this.photoService = photoService;
        }

        public async Task<IActionResult> Active()
        {
            ReportsViewModel reportsViewModel = new ReportsViewModel();
            List<ReportViewModel> reports = new List<ReportViewModel>();

            var reportsDb = await reportService.GetAllActiveReports();

            foreach (var item in reportsDb.Reports)
            {
                ReportViewModel report = new ReportViewModel()
                {
                    Description = item.Description,
                    ReportedSubjectUrl = await photoService.GetPhotoUrl(item.ReportedSubjectId),
                    Id = item.Id,
                    IsResolved = item.IsResolved,
                    ReportedOn = item.ReportedOn
                };

                reports.Add(report);
            }

            reportsViewModel.Reports = reports;

            return View(reportsViewModel);
        }

        public async Task<IActionResult> Report(string reportId)
        {
            ReportViewModel reportViewModel = new ReportViewModel();

            var reportDb = await reportService.GetReport(reportId);

            reportViewModel.Id = reportDb.Id;
            reportViewModel.Description = reportDb.Description;
            reportViewModel.IsResolved = reportDb.IsResolved;
            reportViewModel.ReportedOn = reportDb.ReportedOn;
            reportViewModel.ReportedSubjectUrl = await photoService.GetPhotoUrl(reportDb.ReportedSubjectId);
              
            return View(reportViewModel);
        }

    }
}
