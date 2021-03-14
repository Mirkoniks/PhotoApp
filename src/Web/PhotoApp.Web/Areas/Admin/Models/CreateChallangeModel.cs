using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class CreateChallangeModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Challange start")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Challange end")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Photos upload")]
        public ICollection<IFormFile> Photos { get; set; }
    }
}
