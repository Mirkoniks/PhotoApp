using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class ReportsViewModel
    {
        public IEnumerable<ReportViewModel> Reports { get; set; }
    }
}
