using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Web.ViewModels
{
    public class TopPhotosViewModel
    {
        public IEnumerable<TopPhotoViewModel> Photos { get; set; }

        public int ChallangeId { get; set; }

    }
}
