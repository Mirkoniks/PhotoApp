using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoApp.Data.Models
{
    public class Challange
    {
        public int ChallangeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsOpen { get; set; }

        public bool IsUpcoming { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxPhotos { get; set; }

        public int ChallangeCoverPhotoId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<PhotoChallange> PhotosChallanges { get; set; } = new HashSet<PhotoChallange>();

        public ICollection<UserWonChallange> UserWonChallanges { get; set; } = new HashSet<UserWonChallange>();
    }
}
