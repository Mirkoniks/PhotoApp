using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public int TotalUsersCount { get; set; }

        public int NewUsersCount { get; set; }

        public int TotalPhotosCount { get; set; }

        public int NewPhotosCount { get; set; }

        public int NewReportsCount { get; set; }

        public int UpcomingChallangesCount { get; set; }

        public int OpenChallangeCount { get; set; }

        public int ClosedChallangesCount { get; set; }
    }
}
