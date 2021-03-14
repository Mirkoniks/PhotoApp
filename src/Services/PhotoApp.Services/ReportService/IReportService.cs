using PhotoApp.Services.Models.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoApp.Services.ReportService
{
    public interface IReportService
    {
        public Task<int> GetAllActiveReportsCount();

        public Task<ReportServiceModel> GetReport(string id);

        public Task<ReportsServiceModel> GetAllActiveReports();

        public Task<ReportsServiceModel> GetAllReports();

        public Task CreateReport(ReportServiceModel model);

        public Task CloseReport(string id);

    }
}
