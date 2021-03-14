using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Report
{
    public class ReportServiceModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public int ReportedSubjectId { get; set; }

        public DateTime ReportedOn { get; set; }

        public bool IsResolved { get; set; }
    }
}
