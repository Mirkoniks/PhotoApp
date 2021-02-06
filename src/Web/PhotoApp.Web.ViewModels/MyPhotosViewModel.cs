using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Web.ViewModels
{
    public class MyPhotosViewModel
    {
        public IEnumerable<MyPhotoViewModel> Photos { get; set; }
    }
}
