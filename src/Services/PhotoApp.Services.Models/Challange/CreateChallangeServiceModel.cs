using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class CreateChallangeServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsOpen { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxPhotos { get; set; }
    }
}
