using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PhotoApp.Web.ViewModels
{

    public class CreateChallangeModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Is challange open")]
        public bool IsOpen { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Challange start")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Challange end")]
        public DateTime EndTime { get; set; }
    }
}
