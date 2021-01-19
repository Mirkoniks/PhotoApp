using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class PhotoChallangeServiceModel
    {
        public int ChallangeId { get; set; }

        public int PhotoId { get; set; }

        public int VoteCount { get; set; }
    }
}
