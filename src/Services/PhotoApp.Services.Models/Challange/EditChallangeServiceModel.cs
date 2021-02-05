using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Services.Models.Challange
{
    public class EditChallangeServiceModel
    {
        public int ChallangeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<IFormFile> ChallangeCoverPhoto { get; set; }
    }
}
