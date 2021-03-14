using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class ReportViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string ReportedSubjectUrl { get; set; }

        public DateTime ReportedOn { get; set; }

        public bool IsResolved { get; set; }
    }
}
