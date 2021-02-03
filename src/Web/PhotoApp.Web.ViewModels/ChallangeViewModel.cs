﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Web.ViewModels
{
    public class ChallangeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
     
        public string Description { get; set; }
  
        public bool IsOpen { get; set; }

        public bool IsUpcoming { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string UserId { get; set; }

        public string TopPhotoLink { get; set; }
    }
}
