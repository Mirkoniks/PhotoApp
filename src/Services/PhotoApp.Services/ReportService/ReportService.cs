using PhotoApp.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PhotoApp.Services.Models.Report;
using PhotoApp.Data.Models;

namespace PhotoApp.Services.ReportService
{
    public class ReportService : IReportService
    {
        private readonly PhotoAppDbContext dbContext;

        public ReportService(PhotoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> GetAllActiveReportsCount()
        {
            var count =dbContext.Reports.Where(r => r.IsResolved == false).Count();

            return count;
        }

        public async Task<ReportsServiceModel> GetAllActiveReports()
        {
            ReportsServiceModel reportsServiceModel = new ReportsServiceModel();
            List<ReportServiceModel> reports = new List<ReportServiceModel>();

            var reportsDb = dbContext.Reports.Where(r => r.IsResolved == false).ToList();

            foreach (var reportDb in reportsDb)
            {
                ReportServiceModel serviceModel = new ReportServiceModel()
                {
                    Id = reportDb.Id,
                    ReportedSubjectId = reportDb.ReportedSubjectId,
                    Description = reportDb.Descripton,
                    IsResolved = reportDb.IsResolved,
                    ReportedOn = reportDb.ReportedOn
                };

                reports.Add(serviceModel);
            }

            reportsServiceModel.Reports = reports;

            return reportsServiceModel;
        }

        public async Task<ReportServiceModel> GetReport(string id)
        {
            var reportDb = dbContext.Reports.Where(r => r.Id == id).FirstOrDefault();

            ReportServiceModel serviceModel = new ReportServiceModel()
            {
                Id = reportDb.Id,
                ReportedSubjectId = reportDb.ReportedSubjectId,
                Description = reportDb.Descripton,
                IsResolved = reportDb.IsResolved,
                ReportedOn = reportDb.ReportedOn
            };

            return serviceModel;
        }

        public async Task<ReportsServiceModel> GetAllReports()
        {
            ReportsServiceModel reportsServiceModel = new ReportsServiceModel();
            List<ReportServiceModel> reports = new List<ReportServiceModel>();

            var reportsDb = dbContext.Reports.ToList();

            foreach (var reportDb in reportsDb)
            {
                ReportServiceModel serviceModel = new ReportServiceModel()
                {
                    Id = reportDb.Id,
                    ReportedSubjectId = reportDb.ReportedSubjectId,
                    Description = reportDb.Descripton,
                    IsResolved = reportDb.IsResolved,
                    ReportedOn = reportDb.ReportedOn
                };

                reports.Add(serviceModel);
            }

            reportsServiceModel.Reports = reports;

            return reportsServiceModel;
        }

        public async Task CreateReport(ReportServiceModel model)
        {
            Report report = new Report()
            {
                Id = new Guid().ToString(),
                Descripton = model.Description,
                ReportedSubjectId = model.ReportedSubjectId,
                IsResolved = false,
                ReportedOn = DateTime.UtcNow
            };

            dbContext.Reports.Add(report);

            await dbContext.SaveChangesAsync();
        }

        public async Task CloseReport(string id)
        {
            var report = dbContext.Reports.Where(r => r.Id == id).FirstOrDefault().IsResolved = true;

            await dbContext.SaveChangesAsync();
        }

        private async Task<bool> CheckIfResolved(string reportId)
        {
           var report = dbContext.Reports.Where(r => r.Id == reportId).FirstOrDefault();

            if (report.IsResolved == true)
            {
                return true;
            }

            return false;
        }
    }
}
