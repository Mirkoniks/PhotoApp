using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class ChallangeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StarTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        public string Status { get; set; }

        [Display(Name = "Cover photo")]
        public ICollection<IFormFile> ChallangeCoverPhoto { get; set; }

        public string CoverPhotoLink { get; set; }
    }
}
