using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Web.ViewModels
{
    public class LikedPhotosViewModel
    {
        public IEnumerable<LikedPhotoViewModel> Photos { get; set; }
    }
}
