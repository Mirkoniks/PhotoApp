using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhotoApp.Web.ViewModels
{
    public class EditChallangeViewModel
    {
        public int ChallangeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Start time")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Display(Name ="End time")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Challang cover photo")]
        public ICollection<IFormFile> ChallangeCoverPhoto { get; set; }
    }
}
