using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoApp.Web.Areas.Admin.Models
{
    public class SearchModel
    {
        [Display (Name = "Username")]
        public string Username { get; set; }

        public bool IsValid { get; set; } = true;
    }
}
