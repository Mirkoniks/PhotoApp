using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class AdminChallangeServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsOpen { get; set; }

        public bool IsUpcoming { get; set; }
    }
}
