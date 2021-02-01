using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class TopPhotosServiceModel
    {
        public IEnumerable<TopPhotoServiceModel> Photos { get; set; }
        public int ChallangeId { get; set; } 
    }
}
