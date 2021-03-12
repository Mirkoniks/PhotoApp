using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class AllChalangesViewModel
    {
        public IEnumerable<ChallangeViewModel> Challanges { get; set; }
    }
}
